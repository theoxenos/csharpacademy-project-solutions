import todosApiService from '../services/todosApiService.js';
import mainView from '../views/mainView.js';
import deleteView from '../views/deleteView.js';
import upsertView from '../views/upsertView.js';
import toastView from '../views/toastView.js';
import {Todo, TodoUpsert} from '../types.js';
import {TOAST_TYPE} from "../consts.js";

class TodoController {
    private todos: Todo[];

    constructor() {
        this.todos = [];
    }

    async init() {
        mainView.onDeleteClick = (id: number) => this.handleDeleteClick(id);
        mainView.onCreateClick = () => this.handleCreateClick();
        mainView.onUpdateClick = (id: number) => this.handleUpdateClick(id);
        mainView.onCompleteClick = (id: number) => this.handleToggleComplete(id);

        deleteView.onModalSubmitted = (id: number) => this.handleDeleteConfirm(id);
        upsertView.onModalSubmitted = (todoData: TodoUpsert) => this.handleUpsertConfirm(todoData);

        await this.loadTodos();
    }

    async loadTodos() {
        try {
            this.todos = await todosApiService.getAllTodos();
            mainView.render(this.todos);
        } catch (error) {
            console.error('Failed to load todos:', error);
            toastView.show('Failed to load todos. Please try again later.', TOAST_TYPE.ERROR);
        }
    }

    handleDeleteClick(id: number) {
        deleteView.show(id);
    }

    async handleDeleteConfirm(id: number) {
        try {
            await todosApiService.deleteTodoById(id);
            await this.loadTodos();
            toastView.show('Todo deleted successfully!', TOAST_TYPE.SUCCESS);
        } catch (error) {
            console.error('Failed to delete todo:', error);
            toastView.show('Failed to delete todo. Please try again later.', TOAST_TYPE.ERROR);
        }
    }

    handleCreateClick() {
        upsertView.show();
    }

    handleUpdateClick(id: number) {
        const todo = this.todos.find(t => t.id === id);
        if (todo) {
            upsertView.show(todo);
        }
    }

    async handleUpsertConfirm(todoData: TodoUpsert) {
        try {
            if (todoData.id) {
                await todosApiService.updateTodo(todoData);
            } else {
                await todosApiService.createTodo(todoData);
            }
            await this.loadTodos();
            toastView.show('Todo saved successfully!', TOAST_TYPE.SUCCESS);
        } catch (error) {
            console.error('Failed to upsert todo:', error);
            toastView.show('Failed to save todo. Please try again later.', TOAST_TYPE.ERROR);
        }
    }

    async handleToggleComplete(id: number) {
        const todo = this.todos.find(t => t.id === id);
        if (todo) {
            try {
                const updatedTodo = {...todo, completed: !todo.completed};
                await todosApiService.updateTodo(updatedTodo);
                await this.loadTodos();
                toastView.show('Todo status updated successfully!', TOAST_TYPE.SUCCESS);
            } catch (error) {
                console.error('Failed to toggle complete:', error);
                toastView.show('Failed to update todo status. Please try again later.', TOAST_TYPE.ERROR);
            }
        }
    }
}

export default new TodoController();
