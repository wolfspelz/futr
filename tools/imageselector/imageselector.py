"""
Image Selector Tool
Displays images in a grid GUI. Click to toggle selection, drag to reorder.
The output order matches the final visual order.

Usage:
    echo '<json array>' | python imageselector.py [--data-dir PATH]
    python imageselector.py [--data-dir PATH] < images.json
    python imageselector.py [--data-dir PATH] --file images.json

Input: JSON array of image objects, each with at least:
    - src: URL or /proxy/... path
    - text: caption/description
    (other fields like link, author, license, legal, tags are preserved)

Output: JSON array of selected image objects to stdout (in display order).
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
DRAG_THRESHOLD = 8
SELECT_COLOR = "#2196F3"
DESELECT_COLOR = "#cccccc"
DRAG_SOURCE_COLOR = "#555555"
DRAG_TARGET_COLOR = "#FF9800"
BG_COLOR = "#1e1e1e"
CARD_BG = "#2d2d2d"
TEXT_COLOR = "#e0e0e0"
CAPTION_COLOR = "#b0b0b0"


def resolve_src(src, data_dir):
    """Resolve image src to a fetchable URL or local path."""
    if src.startswith("/proxy/"):
        local = os.path.join(data_dir, "proxy", src[len("/proxy/"):])
        return ("file", local)
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


class ImageItem:
    """Holds all state for one image slot."""
    __slots__ = ("data", "selected", "photo", "frame", "img_label",
                 "idx_label", "cap_label", "src_label")

    def __init__(self, data):
        self.data = data
        self.selected = True
        self.photo = None
        self.frame = None
        self.img_label = None
        self.idx_label = None
        self.cap_label = None
        self.src_label = None


class ImageSelectorApp:
    def __init__(self, root, images, data_dir):
        self.root = root
        self.data_dir = data_dir
        self.items = [ImageItem(img) for img in images]
        self.result = None

        # Drag state
        self._drag_source = None
        self._drag_start_x = 0
        self._drag_start_y = 0
        self._dragging = False
        self._drag_target = None
        self._drag_ghost = None  # Toplevel window showing dragged image
        self._drag_offset_x = 0  # Click offset from card top-left
        self._drag_offset_y = 0

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

        tk.Button(btn_frame, text="Cancel", command=self.on_cancel,
                  bg="#8B0000", fg="white", font=("Segoe UI", 10),
                  padx=10, pady=4, relief=tk.FLAT, cursor="hand2").pack(side=tk.RIGHT, padx=(0, 8))

        # Scrollable canvas
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
        self.canvas.bind_all("<MouseWheel>", self._on_mousewheel)

        # Status bar
        self.status = tk.Label(root, text="Loading images...", bg="#333", fg=CAPTION_COLOR,
                               font=("Segoe UI", 9), anchor=tk.W, padx=8)
        self.status.pack(fill=tk.X, side=tk.BOTTOM)

        self._create_cards()
        self._update_count()
        self._load_images_async()

        self.root.protocol("WM_DELETE_WINDOW", self.on_cancel)
        self.root.bind("<Return>", lambda e: self.on_done())
        self.root.bind("<Escape>", lambda e: self.on_cancel())

    def _on_canvas_configure(self, event):
        self.canvas.itemconfig(self.canvas_window, width=event.width)

    def _on_mousewheel(self, event):
        self.canvas.yview_scroll(int(-1 * (event.delta / 120)), "units")

    # ── Card creation ──────────────────────────────────────────────

    def _create_cards(self):
        """Create card widgets for all items and grid them."""
        for pos, item in enumerate(self.items):
            self._create_single_card(item, pos)

        for c in range(COLUMNS):
            self.grid_frame.columnconfigure(c, weight=1)

    def _create_single_card(self, item, pos):
        """Create widgets for one ImageItem."""
        card = tk.Frame(self.grid_frame, bg=CARD_BG, padx=4, pady=4,
                        highlightbackground=SELECT_COLOR, highlightthickness=3)

        img_label = tk.Label(card, text="Loading...", bg=CARD_BG, fg=CAPTION_COLOR,
                             width=THUMB_SIZE // 7, height=THUMB_SIZE // 14)
        img_label.pack(padx=2, pady=(2, 0))

        idx_label = tk.Label(card, text=f"#{pos + 1}", bg=CARD_BG, fg="#888",
                             font=("Segoe UI", 8))
        idx_label.pack()

        caption = item.data.get("text", item.data.get("src", ""))[:50]
        cap_label = tk.Label(card, text=caption, bg=CARD_BG, fg=CAPTION_COLOR,
                             font=("Segoe UI", 9), wraplength=THUMB_SIZE)
        cap_label.pack(padx=2, pady=(0, 2))

        src = item.data.get("src", "")
        if src.startswith("/proxy/"):
            src_hint = "local proxy"
        else:
            parsed = urlparse(src)
            src_hint = parsed.netloc[:30] if parsed.netloc else src[:30]
        src_label = tk.Label(card, text=src_hint, bg=CARD_BG, fg="#666",
                             font=("Segoe UI", 8))
        src_label.pack(padx=2, pady=(0, 2))

        item.frame = card
        item.img_label = img_label
        item.idx_label = idx_label
        item.cap_label = cap_label
        item.src_label = src_label

        # Bind drag/click events to all widgets in the card
        for widget in [card, img_label, cap_label, idx_label, src_label]:
            widget.bind("<ButtonPress-1>", lambda e, it=item: self._on_press(e, it))
            widget.bind("<B1-Motion>", lambda e, it=item: self._on_motion(e, it))
            widget.bind("<ButtonRelease-1>", lambda e, it=item: self._on_release(e, it))
            widget.configure(cursor="hand2")

        row = pos // COLUMNS
        col = pos % COLUMNS
        card.grid(row=row, column=col, padx=PAD // 2, pady=PAD // 2, sticky="nsew")

    def _regrid(self):
        """Reposition all cards in the grid and update index labels."""
        for pos, item in enumerate(self.items):
            row = pos // COLUMNS
            col = pos % COLUMNS
            item.frame.grid(row=row, column=col, padx=PAD // 2, pady=PAD // 2, sticky="nsew")
            item.idx_label.configure(text=f"#{pos + 1}")

    # ── Drag and drop ──────────────────────────────────────────────

    def _item_index(self, item):
        """Get current position of an item."""
        for i, it in enumerate(self.items):
            if it is item:
                return i
        return -1

    def _pos_from_event(self, event):
        """Convert a mouse event to a grid position index."""
        # Get the mouse position relative to the canvas
        widget = event.widget
        canvas_x = self.canvas.canvasx(widget.winfo_rootx() + event.x - self.canvas.winfo_rootx())
        canvas_y = self.canvas.canvasy(widget.winfo_rooty() + event.y - self.canvas.winfo_rooty())

        # Calculate grid position from canvas coordinates
        if not self.items:
            return 0

        # Get approximate cell dimensions from the first card
        first = self.items[0].frame
        cell_w = first.winfo_width() + PAD
        cell_h = first.winfo_height() + PAD

        if cell_w <= 0 or cell_h <= 0:
            return 0

        col = max(0, min(COLUMNS - 1, int(canvas_x / cell_w)))
        row = max(0, int(canvas_y / cell_h))
        pos = row * COLUMNS + col
        return max(0, min(len(self.items) - 1, pos))

    def _on_press(self, event, item):
        """Record drag start."""
        self._drag_source = item
        self._drag_start_x = event.x_root
        self._drag_start_y = event.y_root
        # Offset from click point to image label top-left
        self._drag_offset_x = event.x_root - item.img_label.winfo_rootx()
        self._drag_offset_y = event.y_root - item.img_label.winfo_rooty()
        self._dragging = False
        self._drag_target = None

    def _on_motion(self, event, item):
        """Handle mouse motion - start drag if past threshold."""
        if self._drag_source is None:
            return

        dx = abs(event.x_root - self._drag_start_x)
        dy = abs(event.y_root - self._drag_start_y)

        if not self._dragging and (dx > DRAG_THRESHOLD or dy > DRAG_THRESHOLD):
            self._dragging = True
            # Dim the source card
            self._drag_source.frame.configure(highlightbackground=DRAG_SOURCE_COLOR,
                                              highlightthickness=3)
            self._create_drag_ghost(event)
            self.status.configure(text="Dragging... drop on target position to reorder")

        if self._dragging:
            # Move ghost to follow cursor
            if self._drag_ghost:
                gx = event.x_root - self._drag_offset_x - 4
                gy = event.y_root - self._drag_offset_y - 4
                self._drag_ghost.geometry(f"+{gx}+{gy}")

            target_pos = self._pos_from_event(event)
            target_item = self.items[target_pos]

            if target_item is not self._drag_target:
                # Clear previous target highlight
                if self._drag_target is not None and self._drag_target is not self._drag_source:
                    self._update_border(self._drag_target)

                # Highlight new target
                if target_item is not self._drag_source:
                    target_item.frame.configure(highlightbackground=DRAG_TARGET_COLOR,
                                                highlightthickness=3)
                self._drag_target = target_item

    def _create_drag_ghost(self, event):
        """Create a floating toplevel showing the dragged image thumbnail."""
        ghost = tk.Toplevel(self.root)
        ghost.overrideredirect(True)
        ghost.attributes("-topmost", True)
        try:
            ghost.attributes("-alpha", 0.8)
        except tk.TclError:
            pass
        ghost.configure(bg=DRAG_TARGET_COLOR)

        if self._drag_source.photo:
            label = tk.Label(ghost, image=self._drag_source.photo, bg=CARD_BG,
                             borderwidth=2, relief=tk.SOLID)
        else:
            caption = self._drag_source.data.get("text", "")[:30]
            label = tk.Label(ghost, text=caption, bg=CARD_BG, fg=TEXT_COLOR,
                             font=("Segoe UI", 9), padx=8, pady=8)
        label.pack(padx=2, pady=2)
        gx = event.x_root - self._drag_offset_x - 4
        gy = event.y_root - self._drag_offset_y - 4
        ghost.geometry(f"+{gx}+{gy}")
        self._drag_ghost = ghost

    def _destroy_drag_ghost(self):
        """Remove the floating drag ghost."""
        if self._drag_ghost:
            self._drag_ghost.destroy()
            self._drag_ghost = None

    def _on_release(self, event, item):
        """Handle mouse release - either toggle or complete drag."""
        self._destroy_drag_ghost()
        if self._dragging and self._drag_source is not None:
            # Complete drag - reorder
            target_pos = self._pos_from_event(event)
            source_pos = self._item_index(self._drag_source)

            if source_pos != target_pos:
                # Move item from source_pos to target_pos
                moved = self.items.pop(source_pos)
                self.items.insert(target_pos, moved)
                self._regrid()

            # Restore borders
            for it in self.items:
                self._update_border(it)

            self.status.configure(
                text=f"Reordered. Click=toggle, Drag=reorder. Enter=Done, Escape=Cancel.")
        elif self._drag_source is not None:
            # Simple click - toggle selection
            idx = self._item_index(self._drag_source)
            if idx >= 0:
                self.items[idx].selected = not self.items[idx].selected
                self._update_border(self.items[idx])
                self._update_count()

        # Reset drag state
        self._drag_source = None
        self._dragging = False
        self._drag_target = None

    # ── Selection ──────────────────────────────────────────────────

    def _update_border(self, item):
        if item.selected:
            item.frame.configure(highlightbackground=SELECT_COLOR, highlightthickness=3)
        else:
            item.frame.configure(highlightbackground=DESELECT_COLOR, highlightthickness=1)

    def _update_count(self):
        count = sum(1 for it in self.items if it.selected)
        self.count_label.configure(text=f"{count} of {len(self.items)} selected")

    def select_all(self):
        for it in self.items:
            it.selected = True
            self._update_border(it)
        self._update_count()

    def deselect_all(self):
        for it in self.items:
            it.selected = False
            self._update_border(it)
        self._update_count()

    # ── Image loading ──────────────────────────────────────────────

    def _load_images_async(self):
        def load_one(item):
            src = item.data.get("src", "")
            return item, load_image(src, self.data_dir)

        def on_all_done(results):
            for item, pil_img in results:
                try:
                    tk_img = ImageTk.PhotoImage(pil_img)
                    item.photo = tk_img
                    item.img_label.configure(image=tk_img, text="",
                                             width=pil_img.width, height=pil_img.height)
                except Exception:
                    pass
            self.status.configure(
                text=f"Loaded {len(results)} images. Click=toggle, Drag=reorder. Enter=Done, Escape=Cancel.")

        def background():
            results = []
            items_snapshot = list(self.items)
            with ThreadPoolExecutor(max_workers=6) as pool:
                futures = {pool.submit(load_one, it): it for it in items_snapshot}
                for future in as_completed(futures):
                    try:
                        results.append(future.result())
                    except Exception:
                        pass
            self.root.after(0, on_all_done, results)

        t = threading.Thread(target=background, daemon=True)
        t.start()

    # ── Done / Cancel ──────────────────────────────────────────────

    def on_done(self):
        self.result = [it.data for it in self.items if it.selected]
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

    if args.data_dir:
        data_dir = args.data_dir
    else:
        script_dir = Path(__file__).resolve().parent
        project_root = script_dir.parent.parent
        data_dir = str(project_root / "data")

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

    root = tk.Tk()

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

    output = json.dumps(app.result, indent=2, ensure_ascii=False)
    sys.stdout.buffer.write(output.encode("utf-8"))
    sys.stdout.buffer.write(b"\n")
    sys.exit(0)


if __name__ == "__main__":
    main()
