// Dynamic content
const categoriesTableBody = document.querySelector('#categoriesTableBody');

// Table buttons
const newCategoryButton = document.querySelector('#btnCategoryNew');

// Upsert modal elements
const editModalElement = document.querySelector('#editModal');

const editModal = new bootstrap.Modal(editModalElement);
const categoryUpsertForm = document.querySelector('#categoryUpsertForm');
const categoryUpsertButton = document.querySelector('#btnCategoryUpdate');

// Delete modal elements
const deleteModalElement = document.querySelector('#confirmationModal');

const deleteModal = new bootstrap.Modal(deleteModalElement);
const deleteCategoryIdElement = document.querySelector('#deleteCategoryId');
const categoryDeleteButton = document.querySelector('#btnCategoryConfirmDelete');

// Event handling       
categoriesTableBody.addEventListener('click', async (event) =>{
    const {target} = event;

    if(!target) {
        return;
    }

    if (target.matches('.edit-category')) {
        if (typeof resetFormValidation === 'function') {
            resetFormValidation(categoryUpsertForm);
        }

        const response = await fetch('/Categories/Detail/' + target.dataset.categoryid)

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const category = await response.json();

        categoryUpsertForm['editCategoryId'].value = category.id;
        categoryUpsertForm['Name'].value = category.name;
        categoryUpsertForm['Color'].value = category.color;

        editModal.show();
    } else if (target.matches('.delete-category')) {
        deleteCategoryIdElement.value = target.dataset.categoryid;

        deleteModal.show();
    }
});

newCategoryButton.addEventListener('click', () => {
    if (typeof resetFormValidation === 'function') {
        resetFormValidation(categoryUpsertForm);
    }
    categoryUpsertForm['editCategoryId'].value = '';
    categoryUpsertForm['Name'].value = '';
    categoryUpsertForm['Color'].value = '#6c757d';

    editModal.show();
});

categoryUpsertForm.addEventListener('submit', async (evt) => {
    evt.preventDefault();

    const todo = {
        id: Number(categoryUpsertForm['editCategoryId'].value),
        name: categoryUpsertForm['Name'].value,
        color: categoryUpsertForm['Color'].value
    };
    const route = todo.id === 0
        ? 'Categories/Create'
        : `/Categories/Update/${todo.id}`;
    const method = todo.id === 0 ? 'POST' : 'PUT';

    const response = await fetch(route, {
        method: method,
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(todo)
    });

    if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
    }

    editModal.hide();
    categoriesTableBody.innerHTML = await response.text();
});

categoryUpsertButton.addEventListener('click', () => {
    // Create a new 'submit' event
    let event = new Event('submit', {
        bubbles: true, // Event will bubble up through the DOM
        cancelable: true // Event can be canceled
    });

    // Dispatch it on the form
    if (categoryUpsertForm.dispatchEvent(event)) {
        editModal.hide(); // Hide the modal only if the event wasn't canceled
    }
});

categoryDeleteButton.addEventListener('click', async () => {
    const response = await fetch('/Categories/Delete/' + Number(deleteCategoryIdElement.value), { method: 'delete'});

    if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
    }

    deleteModal.hide();
    categoriesTableBody.innerHTML = await response.text();
});