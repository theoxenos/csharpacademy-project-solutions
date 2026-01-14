import * as api from './js/api.js';
import * as ui from './js/ui.js';
import { showToast } from './js/toast.js';

let todoToDeleteId = null;

const refreshTodos = async () => {
    try {
        const todos = await api.fetchTodos();
        ui.renderTodos(todos, refreshTodos);
    } catch (error) {
        showToast(error.message, true);
    }
};

const init = async () => {
    document.querySelector('#btnAddTodo').addEventListener('click', ui.showAddModal);

    document.querySelector('#upsertModalSubmit').addEventListener('click', async () => {
        const { id, name, completed } = ui.getTodoFormData();

        if (!name.trim()) {
            showToast('Name is required', true);
            return;
        }

        const isUpdate = id !== '';
        try {
            if (isUpdate) {
                await api.updateTodo(id, { id, name, completed });
            } else {
                await api.createTodo({ name, completed });
            }
            ui.hideUpsertModal();
            await refreshTodos();
            showToast(`${name} ${isUpdate ? 'updated' : 'created'}`);
        } catch (error) {
            showToast(error.message, true);
        }
    });

    ui.setOnDeleteCallback((id) => {
        todoToDeleteId = id;
    });

    document.querySelector('#deleteModalConfirm').addEventListener('click', async () => {
        if (todoToDeleteId === null) return;

        try {
            await api.deleteTodo(todoToDeleteId);
            ui.hideDeleteModal();
            await refreshTodos();
            showToast('Todo deleted');
        } catch (error) {
            showToast(error.message, true);
        } finally {
            todoToDeleteId = null;
        }
    });

    await refreshTodos();
};

void init();
