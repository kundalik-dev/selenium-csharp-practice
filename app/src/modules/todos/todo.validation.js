function validateTodo(req, res, next) {
  const body = req.body;
  if (!body || Array.isArray(body) || typeof body.title !== 'string' || !body.title.trim()) {
    return res.status(400).json({ error: 'title must be a non-empty string' });
  }
  if (body.completed !== undefined && typeof body.completed !== 'boolean') {
    return res.status(400).json({ error: 'completed must be a boolean' });
  }
  req.todoInput = { title: body.title.trim(), completed: body.completed ?? false };
  next();
}

function validateId(req, res, next, id) {
  if (!/^[1-9]\d*$/.test(id) || !Number.isSafeInteger(Number(id))) {
    return res.status(404).json({ error: 'Todo not found' });
  }
  req.todoId = Number(id);
  next();
}

module.exports = { validateTodo, validateId };
