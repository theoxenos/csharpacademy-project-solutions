import elementsService from "../services/elementsService.js";
import {ACTION_TYPE, ActionType} from "../consts.js";
import {Callback, CallbackWithId, Todo} from "../types.js";

class MainView {
    private addTodoButton: HTMLButtonElement;
    private todoList: HTMLElement;
    onPrevPageClick?: Callback;
    onNextPageClick?: Callback;
    private prevPageButton: HTMLButtonElement;

    onCompleteClick?: CallbackWithId;
    onDeleteClick?: CallbackWithId;
    onUpdateClick?: CallbackWithId;
    onCreateClick?: Callback;
    private nextPageButton: HTMLButtonElement;
    private currentPageSpan: HTMLElement;

    constructor() {
        this.addTodoButton = document.querySelector('#btnAddTodo') as HTMLButtonElement;
        this.todoList = document.querySelector('#todoList') as HTMLElement;
        this.prevPageButton = document.querySelector('#prevPage') as HTMLButtonElement;
        this.nextPageButton = document.querySelector('#nextPage') as HTMLButtonElement;
        this.currentPageSpan = document.querySelector('#currentPage') as HTMLElement;

        this.init();
    }

    render(todos: Todo[], page: number, totalTodos: number) {
        this.todoList.innerHTML = '';
        this.todoList.append(...todos.map(elementsService.createTodoItemElement));
        this.currentPageSpan.innerText = page.toString();
        this.prevPageButton.disabled = page === 1;
        this.nextPageButton.disabled = (page * 5) >= totalTodos;
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
        this.prevPageButton.onclick = () => this.onPrevPageClick?.();
        this.nextPageButton.onclick = () => this.onNextPageClick?.();
    }
}

export default new MainView();

