import todosService from "./js/todosApiService.js";
import elementsService from "./js/elementsService.js";

const todoListEl = document.getElementById('todoList');

const renderTodos = (todos) => {
    todoListEl.innerHTML = '';
    
    todos.forEach(todo => {        
        todoListEl.appendChild(elementsService.createTodoItemElement(todo));
    });
}

const initTodos = async () => {
    renderTodos(await todosService.getAllTodos());
}

await initTodos();