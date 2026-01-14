const createElement = (tag, { classes = [], attributes = {}, text = '' } = {}) => {
    const el = document.createElement(tag);
    if (classes.length) el.classList.add(...classes);
    Object.entries(attributes).forEach(([key, value]) => {
        if (key === 'checked') el.checked = value;
        else el.setAttribute(key, value);
    });
    if (text) el.textContent = text;
    return el;
};

const createCheckboxGroup = (todoItem) => {
    const container = createElement('span');
    const checkbox = createElement('input', {
        classes: ['me-2', 'form-check-input'],
        attributes: { type: 'checkbox', checked: todoItem.completed }
    });
    const label = createElement('label', {
        classes: ['form-check-label'],
        text: todoItem.name
    });

    container.append(checkbox, label);
    return container;
};

const createButtonGroup = () => {
    const container = createElement('span', { classes: ['d-flex', 'gap-1'] });
    const updateButton = createElement('button', { classes: ['btn', 'btn-primary', 'bi', 'bi-pencil'] });
    const deleteButton = createElement('button', { classes: ['btn', 'btn-danger', 'bi', 'bi-trash'] });

    container.append(updateButton, deleteButton);
    return container;
};

const createTodoItemElement = (todoItem) => {
    const todoItemEl = createElement('li', {
        classes: ['list-group-item', 'd-flex', 'align-items-center', 'justify-content-between']
    });

    todoItemEl.append(
        createCheckboxGroup(todoItem),
        createButtonGroup()
    );

    return todoItemEl;
};

export default { createTodoItemElement };