class DeleteView {
    constructor() {
        this.modal = new bootstrap.Modal('#deleteModal');
        this.confirmButton = document.querySelector('#deleteModalConfirm');
        this.toDeleteId = null;
        this.onModalSubmitted = () => {
        };

        this.confirmButton.onclick = () => this.handleConfirm();
    }

    setOnModalSubmitted(fn) {
        this.onModalSubmitted = fn;
    }

    show(id) {
        this.toDeleteId = id;
        this.modal.show();
    }

    handleConfirm() {
        this.modal.hide();
        this.onModalSubmitted(this.toDeleteId);
    }
}

export default new DeleteView();