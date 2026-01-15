class UpsertView {
  constructor() {
    this.confirmButton = document.querySelector('#upsertModalSubmit');
    this.modal = new bootstrap.Modal('#upsertModal');
    this.completedInput = document.querySelector('#todo-completed');
    this.nameInput = document.querySelector('#todo-name');
    this.titleHeader = document.querySelector('#upsertLabel');
    this.todoInputId = document.querySelector('#todo-id');
    this.onModalSubmitted = () => {
    };

    this.confirmButton.onclick = () => this.handleConfirm();
  }

  setOnModalSubmitted(callback) {
    this.onModalSubmitted = callback;
  }

  formatTitle(modalType) {
    return `${modalType} Todo`;
  }

  setFormData(todo) {
    if (todo) {
      this.nameInput.value = todo.name;
      this.todoInputId.value = todo.id;
      this.completedInput.checked = todo.completed;
    } else {
      this.nameInput.value = '';
      this.todoInputId.value = '';
      this.completedInput.checked = false;
    }
  }

  show(todo) {
    this.titleHeader.innerText = this.formatTitle(todo ? 'Update' : 'Create');
    this.modal.show();
    this.setFormData(todo);
  }

  handleConfirm() {
    this.modal.hide();
    this.onModalSubmitted({
      name: this.nameInput.value,
      id: this.todoInputId.value,
      completed: this.completedInput.checked
    });
  }
}

export default new UpsertView();