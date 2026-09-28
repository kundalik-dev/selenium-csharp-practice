const { test } = require('node:test');
const assert = require('node:assert/strict');
const { mkdtempSync, rmSync } = require('node:fs');
const { tmpdir } = require('node:os');
const path = require('node:path');
const { once } = require('node:events');
const { openDatabase } = require('../src/database');
const { createApp } = require('../src/app');

async function start(filename) {
  const db = openDatabase(filename);
  const server = createApp(db).listen(0, '127.0.0.1');
  await once(server, 'listening');
  const base = 'http://127.0.0.1:' + server.address().port;
  return {
    request: (route, method = 'GET', body) => fetch(base + route, {
      method,
      headers: { 'Content-Type': 'application/json' },
      body: body === undefined ? undefined : JSON.stringify(body),
    }),
    raw: (body) => fetch(base + '/todos', {
      method: 'POST', headers: { 'Content-Type': 'application/json' }, body,
    }),
    close: () => new Promise((resolve, reject) => {
      server.close((error) => {
        db.close();
        if (error) reject(error);
        else resolve();
      });
      server.closeIdleConnections();
    }),
  };
}

test('CRUD persists across database reopenings; migrations do not reseed deleted data', async () => {
  const directory = mkdtempSync(path.join(tmpdir(), 'todo-api-'));
  const filename = path.join(directory, 'test.sqlite');
  let api;
  try {
    api = await start(filename);
    let response = await api.request('/todos');
    assert.equal(response.status, 200);
    const seeds = await response.json();
    assert.equal(seeds.length, 2);

    const title = "Read O'Reilly; DROP TABLE todos; --";
    response = await api.request('/todos', 'POST', { title });
    assert.equal(response.status, 201);
    const created = await response.json();
    assert.deepEqual(created, { id: 3, title, completed: false });
    assert.equal(response.headers.get('location'), '/todos/3');

    await api.close();
    api = null;
    api = await start(filename);
    response = await api.request('/todos/3');
    assert.equal(response.status, 200);
    assert.deepEqual(await response.json(), created);

    response = await api.request('/todos/3', 'PUT', { title: ' Updated ', completed: true, id: 99 });
    assert.equal(response.status, 200);
    assert.deepEqual(await response.json(), { id: 3, title: 'Updated', completed: true });

    await api.close();
    api = null;
    api = await start(filename);
    assert.deepEqual(await (await api.request('/todos/3')).json(), { id: 3, title: 'Updated', completed: true });
    for (const id of [1, 2, 3]) {
      response = await api.request('/todos/' + id, 'DELETE');
      assert.equal(response.status, 204);
      assert.equal(await response.text(), '');
    }

    await api.close();
    api = null;
    api = await start(filename);
    assert.deepEqual(await (await api.request('/todos')).json(), []);
    assert.equal((await api.request('/todos/3')).status, 404);
    response = await api.request('/todos', 'POST', { title: 'After deletion' });
    assert.equal((await response.json()).id, 4);
  } finally {
    if (api) await api.close();
    // This directory was created exclusively for this test.
    rmSync(directory, { recursive: true, force: true });
  }
});

test('invalid requests return JSON errors without changing stored todos', async () => {
  const api = await start(':memory:');
  try {
    for (const body of [{}, { title: ' ' }, { title: 12 }, [], { title: 'Valid', completed: 'yes' }]) {
      const response = await api.request('/todos', 'POST', body);
      assert.equal(response.status, 400);
      assert.equal(typeof (await response.json()).error, 'string');
    }
    assert.equal((await api.request('/todos/1', 'PUT', { title: '' })).status, 400);
    const malformed = await api.raw('{bad');
    assert.equal(malformed.status, 400);
    assert.deepEqual(await malformed.json(), { error: 'Invalid JSON body' });
    assert.equal((await api.raw(JSON.stringify({ title: 'x'.repeat(110000) }))).status, 413);
    for (const id of ['missing', '0', '-1', '1.5', '99999999999999999999', '999']) {
      assert.equal((await api.request('/todos/' + id)).status, 404);
    }
    assert.equal((await api.request('/todos/999', 'PUT', { title: 'Missing' })).status, 404);
    assert.equal((await api.request('/todos/999', 'DELETE')).status, 404);
    assert.equal((await api.request('/missing')).status, 404);
    assert.deepEqual(await (await api.request('/todos/1')).json(), { id: 1, title: 'Learn API testing', completed: false });
    assert.equal((await (await api.request('/todos')).json()).length, 2);
    const update = await api.request('/todos/2', 'PUT', { title: 'Replacement' });
    assert.equal((await update.json()).completed, false);
  } finally {
    await api.close();
  }
});
