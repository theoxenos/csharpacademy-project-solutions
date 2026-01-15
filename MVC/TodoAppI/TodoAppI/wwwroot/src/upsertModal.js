const confirmButton = document.getElementById('upsertModalSubmit');
const modal = new bootstrap.Modal('#upsertModal');

const completedInput = document.getElementById('todo-completed');
const nameInput = document.getElementById('todo-name');
const titleHeader = document.getElementById('upsertLabel');
const todoInputId = document.getElementById('todo-id');

let onModalSubmitted = () => {};
const setOnModalSubmitted = (callback) => onModalSubmitted = callback;

const formatTitle = (modalType) => {
  return `${modalType} Todo`
}

const setFormData = (todo) => {
  if (todo) {
    nameInput.value = todo.name;
    todoInputId.value = todo.id;
    completedInput.checked = todo.completed;
  } else {
    nameInput.value = '';
    todoInputId.value = '';
    completedInput.checked = false;
  }
}

const show = (todo) => {
  titleHeader.innerText = formatTitle(todo ? 'Update' : 'Create');

  modal.show();
  setFormData(todo);
};

const handleConfirm = () => {
  modal.hide();
  onModalSubmitted({
    name: nameInput.value,
    id: todoInputId.value,
    completed: completedInput.checked
  });
};

const init = () => {
  confirmButton.onclick = handleConfirm;
};

init();

export default {show, setOnModalSubmitted};