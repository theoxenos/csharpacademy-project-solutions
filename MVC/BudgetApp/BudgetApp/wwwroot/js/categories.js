const $categoriesTableBody = $('#categoriesTableBody');
const $newCategoryButton = $('#btnCategoryNew');
const $editModalElement = $('#editModal');
const editModal = new bootstrap.Modal($editModalElement[0]);
const $categoryUpsertForm = $('#categoryUpsertForm');
const $categoryUpsertButton = $('#btnCategoryUpdate');
const $deleteModalElement = $('#confirmationModal');
const deleteModal = new bootstrap.Modal($deleteModalElement[0]);
const $deleteCategoryIdElement = $('#deleteCategoryId');
const $categoryDeleteButton = $('#btnCategoryConfirmDelete');

$categoriesTableBody.on('click', '.edit-category', function () {
    if (typeof resetFormValidation === 'function') {
        resetFormValidation($categoryUpsertForm);
    }

    const categoryId = $(this).data('categoryid');
    $.get('/Categories/Detail/' + categoryId, function (category) {
        $categoryUpsertForm.find('#editCategoryId').val(category.id);
        $categoryUpsertForm.find('#Name').val(category.name);
        $categoryUpsertForm.find('#Color').val(category.color);

        editModal.show();
    }).fail(function (xhr) {
        console.error(`HTTP error! status: ${xhr.status}`);
    });
});

$categoriesTableBody.on('click', '.delete-category', function () {
    $deleteCategoryIdElement.val($(this).data('categoryid'));
    deleteModal.show();
});

$newCategoryButton.click(() => {
    if (typeof resetFormValidation === 'function') {
        resetFormValidation($categoryUpsertForm);
    }
    $categoryUpsertForm[0].reset();
    $categoryUpsertForm.find('#editCategoryId').val(0);

    editModal.show();
});

$categoryUpsertForm.on('submit', function (evt) {
    evt.preventDefault();

    if (!$categoryUpsertForm.valid()) {
        return;
    }

    const todo = {
        id: Number($categoryUpsertForm.find('#editCategoryId').val()),
        name: $categoryUpsertForm.find('#Name').val(),
        color: $categoryUpsertForm.find('#Color').val()
    };

    const route = todo.id === 0
        ? '/Categories/Create'
        : `/Categories/Update/${todo.id}`;
    const method = todo.id === 0 ? 'POST' : 'PUT';

    $.ajax({
        url: route,
        method: method,
        contentType: 'application/json',
        data: JSON.stringify(todo),
        success: function (data) {
            $categoriesTableBody.html(data);
            editModal.hide();
        },
        error: function (xhr) {
            console.error(`HTTP error! status: ${xhr.status}`);
        }
    });
});

$categoryUpsertButton.click(() => {
    $categoryUpsertForm.trigger('submit');
});

$categoryDeleteButton.click(function () {
    const categoryId = Number($deleteCategoryIdElement.val());
    $.ajax({
        url: '/Categories/Delete/' + categoryId,
        method: 'DELETE',
        success: function (data) {
            $categoriesTableBody.html(data);
            deleteModal.hide();
        },
        error: function (xhr) {
            console.error(`HTTP error! status: ${xhr.status}`);
        }
    });
});