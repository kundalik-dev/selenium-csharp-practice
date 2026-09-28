function toTodo(row) {
  return row ? { id: row.id, title: row.title, completed: Boolean(row.completed) } : null;
}

function createTodoRepository(db) {
  const list = db.prepare('SELECT id, title, completed FROM todos ORDER BY id');
  const find = db.prepare('SELECT id, title, completed FROM todos WHERE id = ?');
  const insert = db.prepare('INSERT INTO todos (title, completed) VALUES (?, ?) RETURNING *');
  const update = db.prepare('UPDATE todos SET title = ?, completed = ? WHERE id = ? RETURNING *');
  const remove = db.prepare('DELETE FROM todos WHERE id = ?');

  return {
    list: () => list.all().map(toTodo),
    find: (id) => toTodo(find.get(id)),
    create: ({ title, completed }) => toTodo(insert.get(title, Number(completed))),
    update: (id, { title, completed }) => toTodo(update.get(title, Number(completed), id)),
    remove: (id) => remove.run(id).changes > 0,
  };
}

module.exports = { createTodoRepository };
