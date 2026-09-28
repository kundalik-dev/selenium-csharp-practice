function createTodoController(todos) {
  return {
    list(req, res) {
      res.json(todos.list());
    },
    get(req, res) {
      const todo = todos.find(req.todoId);
      if (!todo) return res.status(404).json({ error: 'Todo not found' });
      res.json(todo);
    },
    create(req, res) {
      const todo = todos.create(req.todoInput);
      res.status(201).location(req.baseUrl + '/' + todo.id).json(todo);
    },
    update(req, res) {
      const todo = todos.update(req.todoId, req.todoInput);
      if (!todo) return res.status(404).json({ error: 'Todo not found' });
      res.json(todo);
    },
    remove(req, res) {
      if (!todos.remove(req.todoId)) return res.status(404).json({ error: 'Todo not found' });
      res.status(204).send();
    },
  };
}

module.exports = { createTodoController };
