import todosService from "./js/todosApiService.js";
import elementsService from "./js/elementsService.js";
import eventBus from "./js/eventBus.js";
import {deleteTodoItemListener} from "./js/eventHandlers.js";

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

eventBus.on('todoDeleted', initTodos);
eventBus.on('todoUpdated', initTodos);
eventBus.on('todoAdded', initTodos);
eventBus.on('todoChecked', initTodos);
eventBus.on('deleteTodo', deleteTodoItemListener)

await initTodos();