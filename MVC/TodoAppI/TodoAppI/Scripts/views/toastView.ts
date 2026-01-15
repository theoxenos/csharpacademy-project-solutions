import {TOAST_TYPE, ToastType} from "../consts.js";

declare var bootstrap: any;

class ToastView {
    private toast;
    private toastContainer = document.querySelector('#toastContainer') as HTMLElement;
    private toastBody: HTMLElement = this.toastContainer.querySelector('.toast-body') as HTMLElement;

    constructor() {
        this.toast = new bootstrap.Toast('#toastContainer');
    }

    show(message: string, type: ToastType = TOAST_TYPE.SUCCESS) {
        this.toastBody.textContent = message;

        this.toastContainer.classList.remove('bg-success', 'bg-danger');
        this.toastContainer.classList.add(`bg-${type}`);

        this.toast.show();
    }
}

export default new ToastView;