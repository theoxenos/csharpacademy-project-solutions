import todosService from "./js/todosApiService.js";
import elementsService from "./js/elementsService.js";
import deleteModal from "./js/deleteModal.js";
import {deleteTodoItemListener, updateTodoItemListener} from "./js/eventHandlers.js";
import upsertModal from "./js/upsertModal.js";

const todoListEl = document.querySelector('#todoList');

let todos = [];

const showDeleteModal = (id) => {
    deleteModal.show();
    deleteModal.setOnModalSubmitted(() => deleteTodoItemListener(id));
}

const showUpdateModal = (todo) => {
    upsertModal.show(todo);
    upsertModal.setOnModalSubmitted(updateTodoItemListener);
};

const handleListCLick = (event) => {
    try {
        if (event.target.dataset.type === 'delete') {
            showDeleteModal(event.target.closest('li').dataset.id);
        } else if (event.target.dataset.type === 'update') {
            const todoListId = Number(event.target.closest('li').dataset.id);
            const toUpdateTodo = todos.find(todo => todo.id === todoListId);
            showUpdateModal(toUpdateTodo);
        }
    } catch (error) {
        console.error(error);
    }
};

const renderTodos = () => {
    todoListEl.innerHTML = '';
    todoListEl.onclick = handleListCLick;
    
    todos.forEach(todo => {        
        todoListEl.appendChild(elementsService.createTodoItemElement(todo));
    });
}

const initTodos = async () => {
    todos = await todosService.getAllTodos();
    renderTodos();
}



await initTodos();