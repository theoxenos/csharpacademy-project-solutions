import {Todo, TodoUpsert} from "./types.js";

declare var bootstrap: any;

class UpsertView {
  private confirmButton: HTMLButtonElement;
  private modal: any;
  private completedInput: HTMLInputElement;
  private nameInput: HTMLInputElement;
  private titleHeader: HTMLElement;
  private todoInputId: HTMLInputElement;
  private onModalSubmitted: (todo: TodoUpsert) => void;

  constructor() {
    this.confirmButton = document.querySelector('#upsertModalSubmit') as HTMLButtonElement;
    this.modal = new bootstrap.Modal('#upsertModal');
    this.completedInput = document.querySelector('#todo-completed') as HTMLInputElement;
    this.nameInput = document.querySelector('#todo-name') as HTMLInputElement;
    this.titleHeader = document.querySelector('#upsertLabel') as HTMLElement;
    this.todoInputId = document.querySelector('#todo-id') as HTMLInputElement;
    this.onModalSubmitted = () => {
    };

    this.confirmButton.onclick = () => this.handleConfirm();
  }

  setOnModalSubmitted(callback: (todo: TodoUpsert) => void) {
    this.onModalSubmitted = callback;
  }

  formatTitle(modalType: string) {
    return `${modalType} Todo`;
  }

  setFormData(todo?: Todo) {
    if (todo) {
      this.nameInput.value = todo.name;
      this.todoInputId.value = String(todo.id);
      this.completedInput.checked = todo.completed;
    } else {
      this.nameInput.value = '';
      this.todoInputId.value = '';
      this.completedInput.checked = false;
    }
  }

  show(todo?: Todo) {
    this.titleHeader.innerText = this.formatTitle(todo ? 'Update' : 'Create');
    this.modal.show();
    this.setFormData(todo);
  }

  handleConfirm() {
    this.modal.hide();
    this.onModalSubmitted({
      name: this.nameInput.value,
      id: Number(this.todoInputId.value) || undefined,
      completed: this.completedInput.checked
    });
  }
}

export default new UpsertView();