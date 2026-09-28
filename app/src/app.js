const express = require('express');
const { createTodoRouter } = require('./modules/todos/todo.routes');
const { notFound, errorHandler } = require('./middleware/errors');

function createApp(db) {
  const app = express();
  app.disable('x-powered-by');
  app.use(express.json({ limit: '100kb' }));
  app.get('/', (req, res) => {
    res.json({ message: 'Todo practice API is running', todos: '/todos' });
  });
  app.use('/todos', createTodoRouter(db));
  // Mount future posts and authentication routers here.
  app.use(notFound);
  app.use(errorHandler);
  return app;
}

module.exports = { createApp };
