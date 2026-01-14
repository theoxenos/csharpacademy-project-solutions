const todoModule = (() => {
    let todos = [];
    let todoToDeleteId = null;

    const toastSuccessElement = document.getElementById('toastSuccess');
    const toastErrorElement = document.getElementById('toastError');
    const upsertModal = new bootstrap.Modal('#upsertModal');
    const deleteModal = new bootstrap.Modal('#deleteModal');
    const upsertModalSubmit = document.querySelector('#upsertModalSubmit');
    const deleteModalConfirm = document.querySelector('#deleteModalConfirm');
    const updateMessage = document.querySelector('#updateMessage');

    const showToast = (message, isError = false) => {
        const toastElement = isError ? toastErrorElement : toastSuccessElement;
        const toastBody = toastElement.querySelector('.toast-body');
        toastBody.textContent = message;
        const toast = bootstrap.Toast.getOrCreateInstance(toastElement);
        toast.show();
    };

    const getElements = () => {
        this.button = document.querySelector('#btnAddTodo');
        this.todoList = document.querySelector('#todoList');
    }
    const addEventHandlers = () => {
        button.addEventListener('click', () => {
            const completedElement = document.querySelector('#todo-completed');
            const nameElement = document.querySelector('#todo-name');
            const todoItemId = document.querySelector('#todoId');

            completedElement.checked = false;
            nameElement.value = '';
            todoItemId.value = '';
            
            updateMessage.classList.add('d-none');
            document.querySelector('#upsertLabel').textContent = 'Add Todo';

            upsertModal.show();
        });

        upsertModalSubmit.addEventListener('click', async () => {
            const nameElement = document.querySelector('#todo-name');
            const completedElement = document.querySelector('#todo-completed');
            const todoItemId = document.querySelector('#todoId').value;

            if (!nameElement.value.trim()) {
                showToast('Name is required', true);
                return;
            }

            const isUpdate = todoItemId !== '';
            const method = isUpdate ? 'put' : 'post';
            const url = isUpdate ? `todos/${todoItemId}` : 'todos';
            const init = {
                method,
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    name: nameElement.value,
                    completed: completedElement.checked
                })
            }
            
            try {
                const response = await fetch(url, init);

                if (response.ok) {
                    upsertModal.hide();
                    await fetchTodos();
                    logTodos();
                    showToast(`${nameElement.value} ${isUpdate ? 'updated' : 'created'}`);
                } else {
                    const errorText = await response.text();
                    showToast(errorText || `Error ${isUpdate ? 'updating' : 'creating'} todo`, true);
                }
            } catch (error) {
                showToast('A network error occurred', true);
            }
        });

        deleteModalConfirm.addEventListener('click', async () => {
            if (todoToDeleteId === null) return;

            try {
                const response = await fetch(`todos/${todoToDeleteId}`, {method: 'delete'});
                if (response.ok) {
                    deleteModal.hide();
                    await fetchTodos();
                    logTodos();
                    showToast('Todo deleted');
                } else {
                    showToast('Error deleting todo', true);
                }
            } catch (error) {
                showToast('A network error occurred', true);
            } finally {
                todoToDeleteId = null;
            }
        });
    };

    const fetchTodos = async () => {
        try {
            const response = await fetch('/todos');
            if (response.ok) {
                todos = await response.json();
            } else {
                showToast('Error fetching todos', true);
            }
        } catch (error) {
            showToast('A network error occurred while fetching todos', true);
        }
    };

    const logTodos = () => {
        this.todoList.innerHTML = '';

        todos.forEach((t) => {
            const todoNode = document.createElement('li');
            todoNode.classList.add('list-group-item', 'd-flex', 'align-items-center', 'justify-content-between');

            const checkboxGroup = document.createElement('span');

            const labelElement = document.createElement('label');
            labelElement.classList.add('ms-2');
            labelElement.textContent = t.name;

            const checkboxElement = document.createElement('input');
            checkboxElement.classList.add('form-check-input')
            checkboxElement.type = 'checkbox';
            checkboxElement.checked = t.completed;

            checkboxElement.addEventListener('click', async (event) => {
                const init = {
                    method: 'put',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({id: t.id, name: t.name, completed: event.target.checked})
                };
                try {
                    const response = await fetch(`/todos/${t.id}`, init);

                    if (response.ok) {
                        showToast(`${t.name} updated`);
                        await fetchTodos();
                        logTodos();
                    } else {
                        showToast('Error updating todo status', true);
                        event.target.checked = !event.target.checked; // Revert checkbox
                    }
                } catch (error) {
                    showToast('A network error occurred', true);
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
                const nameElement = document.querySelector('#todo-name');
                const completedElement = document.querySelector('#todo-completed');
                const todoItemIdElement = document.querySelector('#todoId');

                todoItemIdElement.value = t.id;
                nameElement.value = t.name;
                completedElement.checked = t.completed;

                updateMessage.classList.remove('d-none');
                document.querySelector('#upsertLabel').textContent = 'Update Todo';
                upsertModal.show();
            });

            btnDelete.addEventListener('click', () => {
                todoToDeleteId = t.id;
                deleteModal.show();
            });

            buttonGroup.append(btnUpdate, btnDelete);
            todoNode.appendChild(buttonGroup);

            this.todoList.appendChild(todoNode);
        });
    };

    return {
        init: async () => {
            getElements();
            addEventHandlers();
            await fetchTodos();
            logTodos();
        }
    };
})();

void todoModule.init();
