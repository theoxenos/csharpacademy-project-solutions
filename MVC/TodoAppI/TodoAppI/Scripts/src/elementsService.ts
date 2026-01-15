import {ACTION_TYPE} from "./consts.js";
import {Todo} from "./types.js";

interface CreateElementOptions {
    classes?: string[];
    attributes?: Record<string, any>;
    text?: string;
}

const createElement = (tag: string, {
    classes = [],
    attributes = {},
    text = ''
}: CreateElementOptions = {}): HTMLElement => {
    const el = document.createElement(tag);
    if (classes.length) el.classList.add(...classes);
    Object.entries(attributes).forEach(([key, value]) => {
        if (key === 'checked' && el instanceof HTMLInputElement) el.checked = value;
        else el.setAttribute(key, String(value));
    });
    if (text) el.textContent = text;
    return el;
};

const createCheckboxGroup = (todoItem: Todo) => {
    const container = createElement('span');
    const checkbox = createElement('input', {
        classes: ['me-2', 'form-check-input'],
        attributes: {type: 'checkbox', checked: todoItem.completed, 'data-action-type': ACTION_TYPE.COMPLETE}
    });
    const label = createElement('label', {
        classes: ['form-check-label'],
        text: todoItem.name
    });

    container.append(checkbox, label);
    return container;
};

const createButtonGroup = () => {
    const container = createElement('span', {classes: ['d-flex', 'gap-1']});
    const updateButton = createElement('button', {
        classes: ['btn', 'btn-primary', 'bi', 'bi-pencil'],
        attributes: {'data-action-type': ACTION_TYPE.UPDATE}
    });
    const deleteButton = createElement('button', {
        classes: ['btn', 'btn-danger', 'bi', 'bi-trash'],
        attributes: {'data-action-type': ACTION_TYPE.DELETE}
    });

    container.append(updateButton, deleteButton);
    return container;
};

const createTodoItemElement = (todoItem: Todo) => {
    const todoItemEl = createElement('li', {
        classes: ['list-group-item', 'd-flex', 'align-items-center', 'justify-content-between'],
        attributes: {'data-id': todoItem.id}
    });

    todoItemEl.append(
        createCheckboxGroup(todoItem),
        createButtonGroup()
    );

    return todoItemEl;
};

export default {createTodoItemElement};