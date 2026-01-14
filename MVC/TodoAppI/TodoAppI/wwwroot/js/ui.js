import { updateTodo } from './api.js';
import { showToast } from './toast.js';

const todoList = document.querySelector('#todoList');
const upsertModal = new bootstrap.Modal('#upsertModal');
const deleteModal = new bootstrap.Modal('#deleteModal');
const updateMessage = document.querySelector('#updateMessage');
const upsertLabel = document.querySelector('#upsertLabel');

const todoIdInput = document.querySelector('#todoId');
const todoNameInput = document.querySelector('#todo-name');
const todoCompletedInput = document.querySelector('#todo-completed');

let onDeleteCallback = null;

export const setOnDeleteCallback = (callback) => {
    onDeleteCallback = callback;
};

export const renderTodos = (todos, onRefresh) => {
    todoList.innerHTML = '';

    todos.forEach((t) => {
        const todoNode = document.createElement('li');
        todoNode.classList.add('list-group-item', 'd-flex', 'align-items-center', 'justify-content-between');

        const checkboxGroup = document.createElement('span');

        const labelElement = document.createElement('label');
        labelElement.classList.add('ms-2');
        labelElement.textContent = t.name;

        const checkboxElement = document.createElement('input');
        checkboxElement.classList.add('form-check-input');
        checkboxElement.type = 'checkbox';
        checkboxElement.checked = t.completed;

        checkboxElement.addEventListener('click', async (event) => {
            try {
                await updateTodo(t.id, { id: t.id, name: t.name, completed: event.target.checked });
                showToast(`${t.name} updated`);
                if (onRefresh) await onRefresh();
            } catch (error) {
                showToast(error.message, true);
                event.target.checked = !event.target.checked; // Revert checkbox
            }
        });

        checkboxGroup.appendChild(checkboxElement);
        checkboxGroup.appendChild(labelElement);
        todoNode.appendChild(checkboxGroup);

        const buttonGroup = document.createElement('span');
        const btnUpdate = document.createElement('button');
        const btnDelete = document.createElement('button');

        btnUpdate.classList.add('btn', 'btn-warning', 'bi', 'bi-pencil', 'me-2');
        btnDelete.classList.add('btn', 'btn-danger', 'bi', 'bi-trash');

        btnUpdate.addEventListener('click', () => {
            todoIdInput.value = t.id;
            todoNameInput.value = t.name;
            todoCompletedInput.checked = t.completed;

            updateMessage.classList.remove('d-none');
            upsertLabel.textContent = 'Update Todo';
            upsertModal.show();
        });

        btnDelete.addEventListener('click', () => {
            if (onDeleteCallback) onDeleteCallback(t.id);
            deleteModal.show();
        });

        buttonGroup.append(btnUpdate, btnDelete);
        todoNode.appendChild(buttonGroup);

        todoList.appendChild(todoNode);
    });
};

export const showAddModal = () => {
    todoIdInput.value = '';
    todoNameInput.value = '';
    todoCompletedInput.checked = false;

    updateMessage.classList.add('d-none');
    upsertLabel.textContent = 'Add Todo';
    upsertModal.show();
};

export const hideUpsertModal = () => upsertModal.hide();
export const hideDeleteModal = () => deleteModal.hide();

export const getTodoFormData = () => {
    return {
        id: todoIdInput.value,
        name: todoNameInput.value,
        completed: todoCompletedInput.checked
    };
};
