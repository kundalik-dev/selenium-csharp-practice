const { DatabaseSync } = require('node:sqlite');
const { mkdirSync, readdirSync, readFileSync } = require('node:fs');
const path = require('node:path');

function openDatabase(filename) {
  if (filename !== ':memory:') mkdirSync(path.dirname(filename), { recursive: true });
  const db = new DatabaseSync(filename);
  try {
    db.exec('PRAGMA busy_timeout = 5000; PRAGMA foreign_keys = ON; PRAGMA journal_mode = WAL;');
    db.exec('CREATE TABLE IF NOT EXISTS schema_migrations (name TEXT PRIMARY KEY, applied_at TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP)');
    const directory = path.join(__dirname, 'migrations');
    for (const name of readdirSync(directory).filter((file) => file.endsWith('.sql')).sort()) {
      db.exec('BEGIN IMMEDIATE');
      try {
        if (!db.prepare('SELECT name FROM schema_migrations WHERE name = ?').get(name)) {
          db.exec(readFileSync(path.join(directory, name), 'utf8'));
          db.prepare('INSERT INTO schema_migrations (name) VALUES (?)').run(name);
        }
        db.exec('COMMIT');
      } catch (error) {
        db.exec('ROLLBACK');
        throw error;
      }
    }
    return db;
  } catch (error) {
    db.close();
    throw error;
  }
}

module.exports = { openDatabase };
