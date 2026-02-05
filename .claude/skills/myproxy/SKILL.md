# Proxy External Images

Download external images to the local proxy folder for serving via the FUTR proxy controller.

## When to Use

Use this skill when you need to proxy an image from:
- Wikimedia Commons (upload.wikimedia.org)
- DeviantArt (images-wixmp-*.wixmp.com)
- Other external sources that may block hotlinking

## Proxy Folder Structure

Images are stored in `data/proxy/` with a folder structure based on the source:

```
data/proxy/
├── upload.wikimedia.org/wikipedia/commons/...  (Wikimedia)
├── deviantart.com/{artist}/{filename}          (DeviantArt)
└── {domain}/{path}                             (Other sources)
```

## Steps

### For Wikimedia Commons
```bash
# URL: https://upload.wikimedia.org/wikipedia/commons/a/b1/Image.jpg
# Proxy path mirrors the URL path
mkdir -p data/proxy/upload.wikimedia.org/wikipedia/commons/a/b1
curl -sL "https://upload.wikimedia.org/wikipedia/commons/a/b1/Image.jpg" \
  -o data/proxy/upload.wikimedia.org/wikipedia/commons/a/b1/Image.jpg
```
Use in YAML: `src: /proxy/upload.wikimedia.org/wikipedia/commons/a/b1/Image.jpg`

### For DeviantArt
DeviantArt URLs require a token and are complex. Use a simplified naming scheme:
```bash
# Get the image URL with token from the DeviantArt page using WebFetch
# Then download with a simple filename
mkdir -p data/proxy/deviantart.com/{artist}
curl -sL "{full-url-with-token}" -o data/proxy/deviantart.com/{artist}/{descriptive-name}.jpg
```
Use in YAML: `src: /proxy/deviantart.com/{artist}/{descriptive-name}.jpg`

### For Other Sources
```bash
mkdir -p data/proxy/{domain}/{path}
curl -sL "{url}" -o data/proxy/{domain}/{path}/{filename}
```

## Important Notes

1. Always verify the download succeeded by checking file size (should be >1KB for images)
2. DeviantArt tokens may expire - if download fails, fetch a fresh URL from the DeviantArt page
3. Keep the `link` field in YAML pointing to the original source for attribution
4. After adding images, run `/myreload` to refresh the server
5. Add a `permission` field in YAML with value 'request' indicating that the permission is to be requested. 
