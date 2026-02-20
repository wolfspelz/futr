"""
Image Selector Tool
Displays images in a grid GUI and lets the user select which ones to keep.

Usage:
    echo '<json array>' | python imageselector.py [--data-dir PATH]
    python imageselector.py [--data-dir PATH] < images.json
    python imageselector.py [--data-dir PATH] --file images.json

Input: JSON array of image objects, each with at least:
    - src: URL or /proxy/... path
    - text: caption/description
    (other fields like link, author, license, legal, tags are preserved)

Output: JSON array of selected image objects to stdout.
Exit code 0 = success, 1 = cancelled or error.
"""

import argparse
import io
import json
import os
import sys
import threading
from concurrent.futures import ThreadPoolExecutor, as_completed
from pathlib import Path
from urllib.parse import urlparse

import requests
from PIL import Image, ImageDraw, ImageTk

import tkinter as tk
from tkinter import ttk

THUMB_SIZE = 200
COLUMNS = 4
PAD = 8
SELECT_COLOR = "#2196F3"
DESELECT_COLOR = "#cccccc"
BG_COLOR = "#1e1e1e"
CARD_BG = "#2d2d2d"
TEXT_COLOR = "#e0e0e0"
CAPTION_COLOR = "#b0b0b0"


def resolve_src(src, data_dir):
    """Resolve image src to a fetchable URL or local path."""
    if src.startswith("/proxy/"):
        local = os.path.join(data_dir, "proxy", src[len("/proxy/"):])
        if os.path.exists(local):
            return ("file", local)
        return ("file", local)  # still try, will fail gracefully
    if src.startswith("http://") or src.startswith("https://"):
        return ("url", src)
    if os.path.exists(src):
        return ("file", src)
    return ("url", src)


def load_image(src, data_dir, thumb_size=THUMB_SIZE):
    """Load an image from URL or file path, return PIL Image thumbnail."""
    kind, path = resolve_src(src, data_dir)
    try:
        if kind == "file":
            img = Image.open(path)
        else:
            resp = requests.get(path, timeout=15, headers={
                "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) FUTR-ImageSelector/1.0"
            })
            resp.raise_for_status()
            img = Image.open(io.BytesIO(resp.content))

        img = img.convert("RGBA")
        img.thumbnail((thumb_size, thumb_size), Image.LANCZOS)

        # Paste onto opaque background
        bg = Image.new("RGBA", img.size, (45, 45, 45, 255))
        bg.paste(img, mask=img.split()[3] if img.mode == "RGBA" else None)
        return bg.convert("RGB")
    except Exception as e:
        return make_placeholder(str(e)[:40], thumb_size)


def make_placeholder(message, size=THUMB_SIZE):
    """Create a placeholder image with error text."""
    img = Image.new("RGB", (size, size), (60, 60, 60))
    draw = ImageDraw.Draw(img)
    draw.text((10, size // 2 - 10), "No image", fill=(150, 150, 150))
    draw.text((10, size // 2 + 10), message, fill=(100, 100, 100))
    return img


class ImageSelectorApp:
    def __init__(self, root, images, data_dir):
        self.root = root
        self.images = images
        self.data_dir = data_dir
        self.selected = [True] * len(images)  # all selected by default
        self.photo_images = [None] * len(images)
        self.cards = []
        self.result = None

        self.root.title(f"Image Selector - {len(images)} images")
        self.root.configure(bg=BG_COLOR)
        self.root.minsize(900, 600)

        # Top bar
        top = tk.Frame(root, bg=BG_COLOR)
        top.pack(fill=tk.X, padx=PAD, pady=(PAD, 0))

        tk.Label(top, text=f"{len(images)} candidate images",
                 bg=BG_COLOR, fg=TEXT_COLOR, font=("Segoe UI", 12, "bold")).pack(side=tk.LEFT)

        self.count_label = tk.Label(top, text="", bg=BG_COLOR, fg=SELECT_COLOR,
                                    font=("Segoe UI", 11))
        self.count_label.pack(side=tk.LEFT, padx=20)

        btn_frame = tk.Frame(top, bg=BG_COLOR)
        btn_frame.pack(side=tk.RIGHT)

        self.done_btn = tk.Button(btn_frame, text="Done", command=self.on_done,
                                  bg=SELECT_COLOR, fg="white", font=("Segoe UI", 11, "bold"),
                                  padx=20, pady=4, relief=tk.FLAT, cursor="hand2")
        self.done_btn.pack(side=tk.RIGHT, padx=(8, 0))

        tk.Button(btn_frame, text="Deselect All", command=self.deselect_all,
                  bg="#555", fg="white", font=("Segoe UI", 10),
                  padx=10, pady=4, relief=tk.FLAT, cursor="hand2").pack(side=tk.RIGHT, padx=(8, 0))

        tk.Button(btn_frame, text="Select All", command=self.select_all,
                  bg="#555", fg="white", font=("Segoe UI", 10),
                  padx=10, pady=4, relief=tk.FLAT, cursor="hand2").pack(side=tk.RIGHT)

        # Cancel button
        tk.Button(btn_frame, text="Cancel", command=self.on_cancel,
                  bg="#8B0000", fg="white", font=("Segoe UI", 10),
                  padx=10, pady=4, relief=tk.FLAT, cursor="hand2").pack(side=tk.RIGHT, padx=(0, 8))

        # Scrollable canvas for image grid
        container = tk.Frame(root, bg=BG_COLOR)
        container.pack(fill=tk.BOTH, expand=True, padx=PAD, pady=PAD)

        self.canvas = tk.Canvas(container, bg=BG_COLOR, highlightthickness=0)
        scrollbar = ttk.Scrollbar(container, orient=tk.VERTICAL, command=self.canvas.yview)
        self.canvas.configure(yscrollcommand=scrollbar.set)

        scrollbar.pack(side=tk.RIGHT, fill=tk.Y)
        self.canvas.pack(side=tk.LEFT, fill=tk.BOTH, expand=True)

        self.grid_frame = tk.Frame(self.canvas, bg=BG_COLOR)
        self.canvas_window = self.canvas.create_window((0, 0), window=self.grid_frame, anchor=tk.NW)

        self.grid_frame.bind("<Configure>", lambda e: self.canvas.configure(
            scrollregion=self.canvas.bbox("all")))
        self.canvas.bind("<Configure>", self._on_canvas_configure)

        # Mouse wheel scrolling
        self.canvas.bind_all("<MouseWheel>", self._on_mousewheel)

        # Status bar
        self.status = tk.Label(root, text="Loading images...", bg="#333", fg=CAPTION_COLOR,
                               font=("Segoe UI", 9), anchor=tk.W, padx=8)
        self.status.pack(fill=tk.X, side=tk.BOTTOM)

        # Create card placeholders
        self._create_cards()
        self._update_count()

        # Load images in background
        self._load_images_async()

        # Handle window close
        self.root.protocol("WM_DELETE_WINDOW", self.on_cancel)

        # Keyboard shortcuts
        self.root.bind("<Return>", lambda e: self.on_done())
        self.root.bind("<Escape>", lambda e: self.on_cancel())

    def _on_canvas_configure(self, event):
        self.canvas.itemconfig(self.canvas_window, width=event.width)

    def _on_mousewheel(self, event):
        self.canvas.yview_scroll(int(-1 * (event.delta / 120)), "units")

    def _create_cards(self):
        """Create card frames for each image."""
        for idx, img_data in enumerate(self.images):
            row = idx // COLUMNS
            col = idx % COLUMNS

            card = tk.Frame(self.grid_frame, bg=CARD_BG, padx=4, pady=4,
                            highlightbackground=SELECT_COLOR, highlightthickness=3)
            card.grid(row=row, column=col, padx=PAD // 2, pady=PAD // 2, sticky="nsew")

            # Image label (placeholder first)
            img_label = tk.Label(card, text="Loading...", bg=CARD_BG, fg=CAPTION_COLOR,
                                 width=THUMB_SIZE // 7, height=THUMB_SIZE // 14)
            img_label.pack(padx=2, pady=(2, 0))

            # Index label
            idx_label = tk.Label(card, text=f"#{idx + 1}", bg=CARD_BG, fg="#888",
                                 font=("Segoe UI", 8))
            idx_label.pack()

            # Caption
            caption = img_data.get("text", img_data.get("src", ""))[:50]
            cap_label = tk.Label(card, text=caption, bg=CARD_BG, fg=CAPTION_COLOR,
                                 font=("Segoe UI", 9), wraplength=THUMB_SIZE)
            cap_label.pack(padx=2, pady=(0, 2))

            # Source hint
            src = img_data.get("src", "")
            if src.startswith("/proxy/"):
                src_hint = "local proxy"
            else:
                parsed = urlparse(src)
                src_hint = parsed.netloc[:30] if parsed.netloc else src[:30]
            src_label = tk.Label(card, text=src_hint, bg=CARD_BG, fg="#666",
                                 font=("Segoe UI", 8))
            src_label.pack(padx=2, pady=(0, 2))

            # Click binding
            for widget in [card, img_label, cap_label, idx_label, src_label]:
                widget.bind("<Button-1>", lambda e, i=idx: self.toggle(i))
                widget.configure(cursor="hand2")

            self.cards.append({
                "frame": card,
                "img_label": img_label,
                "idx": idx,
            })

        # Configure grid columns to expand equally
        for c in range(COLUMNS):
            self.grid_frame.columnconfigure(c, weight=1)

    def _load_images_async(self):
        """Load all images in background threads."""
        def load_one(idx):
            src = self.images[idx].get("src", "")
            return idx, load_image(src, self.data_dir)

        def on_all_done(results):
            for idx, pil_img in results:
                try:
                    tk_img = ImageTk.PhotoImage(pil_img)
                    self.photo_images[idx] = tk_img  # prevent GC
                    card = self.cards[idx]
                    card["img_label"].configure(image=tk_img, text="",
                                                width=pil_img.width, height=pil_img.height)
                except Exception:
                    pass
            self.status.configure(text=f"Loaded {len(results)} images. Click to toggle selection. Enter=Done, Escape=Cancel.")

        def background():
            results = []
            with ThreadPoolExecutor(max_workers=6) as pool:
                futures = {pool.submit(load_one, i): i for i in range(len(self.images))}
                for future in as_completed(futures):
                    try:
                        results.append(future.result())
                    except Exception:
                        pass
            # Update UI from main thread
            self.root.after(0, on_all_done, results)

        t = threading.Thread(target=background, daemon=True)
        t.start()

    def toggle(self, idx):
        """Toggle selection of image at index."""
        self.selected[idx] = not self.selected[idx]
        self._update_card_border(idx)
        self._update_count()

    def _update_card_border(self, idx):
        card = self.cards[idx]["frame"]
        if self.selected[idx]:
            card.configure(highlightbackground=SELECT_COLOR, highlightthickness=3)
        else:
            card.configure(highlightbackground=DESELECT_COLOR, highlightthickness=1)

    def _update_count(self):
        count = sum(self.selected)
        self.count_label.configure(text=f"{count} of {len(self.images)} selected")

    def select_all(self):
        for i in range(len(self.images)):
            self.selected[i] = True
            self._update_card_border(i)
        self._update_count()

    def deselect_all(self):
        for i in range(len(self.images)):
            self.selected[i] = False
            self._update_card_border(i)
        self._update_count()

    def on_done(self):
        self.result = [img for img, sel in zip(self.images, self.selected) if sel]
        self.root.destroy()

    def on_cancel(self):
        self.result = None
        self.root.destroy()


def main():
    parser = argparse.ArgumentParser(description="Image Selector GUI")
    parser.add_argument("--data-dir", default=None,
                        help="Path to data/ directory for resolving /proxy/ paths")
    parser.add_argument("--file", "-f", default=None,
                        help="Read image JSON from file instead of stdin")
    args = parser.parse_args()

    # Determine data directory
    if args.data_dir:
        data_dir = args.data_dir
    else:
        # Default: look for data/ relative to this script's grandparent (project root)
        script_dir = Path(__file__).resolve().parent
        project_root = script_dir.parent.parent
        data_dir = str(project_root / "data")

    # Read input
    if args.file:
        with open(args.file, "r", encoding="utf-8") as f:
            raw = f.read()
    else:
        if sys.stdin.isatty():
            print("Error: No input. Pipe JSON or use --file.", file=sys.stderr)
            sys.exit(1)
        raw = sys.stdin.read()

    try:
        images = json.loads(raw)
    except json.JSONDecodeError as e:
        print(f"Error: Invalid JSON: {e}", file=sys.stderr)
        sys.exit(1)

    if not isinstance(images, list) or len(images) == 0:
        print("Error: Input must be a non-empty JSON array.", file=sys.stderr)
        sys.exit(1)

    # Launch GUI
    root = tk.Tk()

    # Set window size and center
    screen_w = root.winfo_screenwidth()
    screen_h = root.winfo_screenheight()
    win_w = min(1200, screen_w - 100)
    win_h = min(800, screen_h - 100)
    x = (screen_w - win_w) // 2
    y = (screen_h - win_h) // 2
    root.geometry(f"{win_w}x{win_h}+{x}+{y}")

    app = ImageSelectorApp(root, images, data_dir)
    root.mainloop()

    if app.result is None:
        print("Cancelled.", file=sys.stderr)
        sys.exit(1)

    # Output selected images as JSON (force UTF-8 on Windows)
    output = json.dumps(app.result, indent=2, ensure_ascii=False)
    sys.stdout.buffer.write(output.encode("utf-8"))
    sys.stdout.buffer.write(b"\n")
    sys.exit(0)


if __name__ == "__main__":
    main()
