import todosApiService from './todosApiService.js';
import mainView from './mainView.js';
import deleteView from './deleteView.js';
import upsertView from './upsertView.js';

class TodoController {
    constructor() {
        this.todos = [];
    }

    async init() {
        mainView.onDeleteClick(id => this.handleDeleteClick(id));
        mainView.onCreateClick(() => this.handleCreateClick());
        mainView.onUpdateClick(id => this.handleUpdateClick(id));
        mainView.onCompletedClick(id => this.handleToggleComplete(id));

        deleteView.setOnModalSubmitted(id => this.handleDeleteConfirm(id));
        upsertView.setOnModalSubmitted(todoData => this.handleUpsertConfirm(todoData));

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

    handleDeleteClick(id) {
        deleteView.show(id);
    }

    async handleDeleteConfirm(id) {
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

    handleUpdateClick(id) {
        const todo = this.todos.find(t => t.id === id);
        if (todo) {
            upsertView.show(todo);
        }
    }

    async handleUpsertConfirm(todoData) {
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

    async handleToggleComplete(id) {
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
