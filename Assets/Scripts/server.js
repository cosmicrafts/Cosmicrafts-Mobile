const http = require('http');
const fs = require('fs');
const path = require('path');
const zlib = require('zlib');

const server = http.createServer((req, res) => {
  let filePath = path.join(__dirname, req.url === '/' ? 'index.html' : req.url);
  let ext = path.extname(filePath);

  // Define MIME types
  const mimeTypes = {
    '.html': 'text/html',
    '.js': 'application/javascript',
    '.wasm': 'application/wasm',
    '.gz': 'application/gzip',
  };

  let contentType = mimeTypes[ext] || 'application/octet-stream';

  // Check if file is gzipped
  if (filePath.endsWith('.gz')) {
    const originalExt = path.extname(filePath.slice(0, -3));
    contentType = mimeTypes[originalExt] || 'application/octet-stream';
    res.setHeader('Content-Encoding', 'gzip');
  }

  res.setHeader('Content-Type', contentType);
  res.setHeader('Cross-Origin-Opener-Policy', 'same-origin');
  res.setHeader('Cross-Origin-Embedder-Policy', 'require-corp');

  // Read and serve the file
  fs.readFile(filePath, (err, data) => {
    if (err) {
      res.statusCode = 404;
      res.end('Not Found');
    } else {
      res.statusCode = 200;
      res.end(data);
    }
  });
});

server.listen(8000, () => {
  console.log('Server running at http://127.0.0.1:8000/');
});
