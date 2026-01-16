import todoController from "./controllers/todoController.js";

// await Promise.all(allTodos.map(async (name) => todosApiService.createTodo({name, completed: false})))

await todoController.init();