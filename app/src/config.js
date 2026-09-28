const path = require('node:path');
const { existsSync } = require('node:fs');

const appRoot = path.resolve(__dirname, '..');
const envPath = path.join(appRoot, '.env');
if (existsSync(envPath)) process.loadEnvFile(envPath);

const port = Number(process.env.PORT || 3000);
if (!Number.isInteger(port) || port < 1 || port > 65535) {
  throw new Error('PORT must be an integer between 1 and 65535');
}

module.exports = {
  port,
  host: process.env.HOST || '127.0.0.1',
  databasePath: path.resolve(appRoot, process.env.DATABASE_PATH || 'data/todos.sqlite'),
};
