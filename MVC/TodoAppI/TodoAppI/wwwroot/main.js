import todosService from "./src/todosApiService.js";
import elementsService from "./src/elementsService.js";
import deleteModal from "./src/deleteModal.js";
import {addTodoItemListener, deleteTodoItemListener, updateTodoItemListener} from "./src/eventHandlers.js";
import upsertModal from "./src/upsertModal.js";
import {ACTION_TYPE} from "./src/consts.js";

const addTodoButton = document.querySelector('#btnAddTodo');
const todoListEl = document.querySelector('#todoList');

let todos = [];

const showDeleteModal = async (id) => {
    deleteModal.show();
    deleteModal.setOnModalSubmitted(() => deleteTodoItemListener(id));
    await fetchTodos();
}

const showUpdateModal = async (todo) => {
    upsertModal.show(todo);
    upsertModal.setOnModalSubmitted(updateTodoItemListener);
    await fetchTodos();
};

const handleAddTodoClick = async () => {
    upsertModal.show();
    upsertModal.setOnModalSubmitted(addTodoItemListener);
    await fetchTodos();
}

const handleToggleCompleteTodo = async (todo) => {
    await updateTodoItemListener({...todo, completed: !todo.completed});
    await fetchTodos();
};

const handleListCLick = async (event) => {
    try {
        const type = event.target.dataset.actionType;
        if (!type) return;

        const li = event.target.closest('li');
        const id = li ? Number(li.dataset.id) : null;
        const todo = id ? todos.find(t => t.id === id) : null;

        switch (type) {
            case ACTION_TYPE.DELETE:
                await showDeleteModal(id);
                break;
            case ACTION_TYPE.UPDATE:
                await showUpdateModal(todo);
                break;
            case ACTION_TYPE.COMPLETE:
                await handleToggleCompleteTodo(todo);
                break;
        }
    } catch (error) {
        console.error(error);
    }
};

const renderTodos = () => {
    todoListEl.innerHTML = '';

    todos.forEach(todo => {
        todoListEl.appendChild(elementsService.createTodoItemElement(todo));
    });
}

const fetchTodos = async () => {
    todos = await todosService.getAllTodos();
    renderTodos();
}

const setUiListeners = () => {
    addTodoButton.onclick = handleAddTodoClick;
    todoListEl.onclick = handleListCLick;
}

const init = async () => {
    await fetchTodos();
    setUiListeners();
}

await init();