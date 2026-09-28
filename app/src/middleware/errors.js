function notFound(req, res) {
  res.status(404).json({ error: 'Route not found' });
}

function errorHandler(error, req, res, next) {
  if (res.headersSent) return next(error);
  if (error.type === 'entity.parse.failed') {
    return res.status(400).json({ error: 'Invalid JSON body' });
  }
  const status = error.status >= 400 && error.status < 600 ? error.status : 500;
  if (status >= 500) console.error(error);
  res.status(status).json({ error: status === 413 ? 'Request body too large' : 'Request failed' });
}

module.exports = { notFound, errorHandler };
