import elementsService from "../services/elementsService.js";
import {ACTION_TYPE, ActionType} from "../consts.js";
import {Callback, CallbackWithId, Todo} from "../types.js";

class MainView {
    private addTodoButton: HTMLButtonElement;
    private todoList: HTMLElement;

    onCompleteClick?: CallbackWithId;
    onDeleteClick?: CallbackWithId;
    onUpdateClick?: CallbackWithId;
    onCreateClick?: Callback;

    constructor() {
        this.addTodoButton = document.querySelector('#btnAddTodo') as HTMLButtonElement;
        this.todoList = document.querySelector('#todoList') as HTMLElement;

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

            switch (actionType) {
                case ACTION_TYPE.COMPLETE:
                    this.onCompleteClick?.(todoId);
                    break;
                case ACTION_TYPE.DELETE:
                    this.onDeleteClick?.(todoId);
                    break;
                case ACTION_TYPE.UPDATE:
                    this.onUpdateClick?.(todoId);
                    break;
            }
        });

        this.addTodoButton.onclick = () => this.onCreateClick?.();
    }
}

export default new MainView();

