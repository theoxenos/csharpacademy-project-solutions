import todosApiService from './todosApiService.js';
import mainView from './mainView.js';
import deleteView from './deleteView.js';
import upsertView from './upsertView.js';
import {Todo, TodoUpsert} from './types.js';

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
        }
    }

    handleDeleteClick(id: number) {
        deleteView.show(id);
    }

    async handleDeleteConfirm(id: number) {
        try {
            await todosApiService.deleteTodoById(id);
            await this.loadTodos();
        } catch (error) {
            console.error('Failed to delete todo:', error);
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
        } catch (error) {
            console.error('Failed to upsert todo:', error);
        }
    }

    async handleToggleComplete(id: number) {
        const todo = this.todos.find(t => t.id === id);
        if (todo) {
            try {
                const updatedTodo = {...todo, completed: !todo.completed};
                await todosApiService.updateTodo(updatedTodo);
                await this.loadTodos();
            } catch (error) {
                console.error('Failed to toggle complete:', error);
            }
        }
    }
}

export default new TodoController();
