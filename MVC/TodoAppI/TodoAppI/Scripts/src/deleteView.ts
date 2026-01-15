declare var bootstrap: any;

class DeleteView {
    private modal: any;
    private confirmButton: HTMLButtonElement;
    private toDeleteId: number | null;
    private onModalSubmitted: (id: number) => void;

    constructor() {
        this.modal = new bootstrap.Modal('#deleteModal');
        this.confirmButton = document.querySelector('#deleteModalConfirm') as HTMLButtonElement;
        this.toDeleteId = null;
        this.onModalSubmitted = () => {
        };

        this.confirmButton.onclick = () => this.handleConfirm();
    }

    setOnModalSubmitted(fn: (id: number) => void) {
        this.onModalSubmitted = fn;
    }

    show(id: number) {
        this.toDeleteId = id;
        this.modal.show();
    }

    handleConfirm() {
        this.modal.hide();
        if (this.toDeleteId !== null) {
            this.onModalSubmitted(this.toDeleteId);
        }
    }
}

export default new DeleteView();