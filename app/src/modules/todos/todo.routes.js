const { Router } = require('express');
const { createTodoRepository } = require('./todo.repository');
const { createTodoController } = require('./todo.controller');
const { validateTodo, validateId } = require('./todo.validation');

function createTodoRouter(db) {
  const router = Router();
  const controller = createTodoController(createTodoRepository(db));
  router.param('id', validateId);
  router.get('/', controller.list);
  router.get('/:id', controller.get);
  router.post('/', validateTodo, controller.create);
  router.put('/:id', validateTodo, controller.update);
  router.delete('/:id', controller.remove);
  return router;
}

module.exports = { createTodoRouter };
