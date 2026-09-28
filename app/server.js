const config = require('./src/config');
const { openDatabase } = require('./src/database');
const { createApp } = require('./src/app');

const db = openDatabase(config.databasePath);
const server = createApp(db).listen(config.port, config.host, () => {
  console.log('Todo API running at http://' + config.host + ':' + config.port);
});

server.on('error', (error) => {
  console.error(error.message);
  db.close();
  process.exitCode = 1;
});

let stopping = false;
function shutdown() {
  if (stopping) return;
  stopping = true;
  server.close(() => db.close());
  server.closeIdleConnections();
}
process.on('SIGINT', shutdown);
process.on('SIGTERM', shutdown);
