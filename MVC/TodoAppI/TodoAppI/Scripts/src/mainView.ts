import elementsService from "./elementsService.js";
import {ACTION_TYPE, ActionType} from "./consts.js";
import {Todo} from "./types.js";

class MainView {
    private addTodoButton: HTMLButtonElement;
    private todoList: HTMLElement;
    private events: Record<ActionType, (id?: any) => void>;

    constructor() {
        this.addTodoButton = document.querySelector('#btnAddTodo') as HTMLButtonElement;
        this.todoList = document.querySelector('#todoList') as HTMLElement;

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

    render(todos: Todo[]) {
        this.todoList.innerHTML = '';
        this.todoList.append(...todos.map(elementsService.createTodoItemElement));
    }

    init() {
        this.todoList.addEventListener('click', (event: MouseEvent) => {
            const target = event.target as HTMLElement;
            const actionEl = target.closest('[data-action-type]');
            if (!actionEl) return;

            const actionType = actionEl.getAttribute('data-action-type') as ActionType;
            const li = actionEl.closest('li');
            if (!li) return;

            const todoId = Number(li.getAttribute('data-id'));
            if (actionType && this.events[actionType]) this.events[actionType](todoId);
        });

        this.addTodoButton.onclick = () => this.events[ACTION_TYPE.CREATE]();
    }

    onCompletedClick(fn: (id: number) => void) {
        this.events[ACTION_TYPE.COMPLETE] = fn;
    }

    onDeleteClick(fn: (id: number) => void) {
        this.events[ACTION_TYPE.DELETE] = fn;
    }

    onUpdateClick(fn: (id: number) => void) {
        this.events[ACTION_TYPE.UPDATE] = fn;
    }

    onCreateClick(fn: () => void) {
        this.events[ACTION_TYPE.CREATE] = fn;
    }
}

export default new MainView();

