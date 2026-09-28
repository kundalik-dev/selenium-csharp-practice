CREATE TABLE todos (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  title TEXT NOT NULL CHECK (length(trim(title)) > 0),
  completed INTEGER NOT NULL DEFAULT 0 CHECK (completed IN (0, 1))
) STRICT;

-- Initial practice data is inserted only when this migration first runs.
INSERT INTO todos (title, completed) VALUES
  ('Learn API testing', 0),
  ('Create a todo', 1);
