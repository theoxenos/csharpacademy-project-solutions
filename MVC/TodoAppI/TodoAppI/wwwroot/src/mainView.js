import elementsService from "./elementsService.js";
import {ACTION_TYPE} from "./consts.js";

class MainView {
    constructor() {
        this.addTodoButton = document.querySelector('#btnAddTodo');
        this.todoList = document.querySelector('#todoList');

        this.events = {
            [ACTION_TYPE.COMPLETE]: () => {
            },
            [ACTION_TYPE.DELETE]: () => {
            },
            [ACTION_TYPE.UPDATE]: () => {
            },
            [ACTION_TYPE.CREATE]: () => {
            }
        };

        this.init();
    }

    render(todos) {
        this.todoList.innerHTML = '';
        this.todoList.append(...todos.map(elementsService.createTodoItemElement));
    }

    init() {
        this.todoList.addEventListener('click', ({target}) => {
            const actionEl = target.closest('[data-action-type]');
            if (!actionEl) return;

            const actionType = actionEl.getAttribute('data-action-type');
            const li = actionEl.closest('li');
            if (!li) return;

            const todoId = Number(li.getAttribute('data-id'));
            if (actionType) this.events[actionType](todoId);
        });

        this.addTodoButton.onclick = () => this.events[ACTION_TYPE.CREATE]();
    }

    onCompletedClick(fn) {
        this.events[ACTION_TYPE.COMPLETE] = fn;
    }

    onDeleteClick(fn) {
        this.events[ACTION_TYPE.DELETE] = fn;
    }

    onUpdateClick(fn) {
        this.events[ACTION_TYPE.UPDATE] = fn;
    }

    onCreateClick(fn) {
        this.events[ACTION_TYPE.CREATE] = fn;
    }
}

export default new MainView();

