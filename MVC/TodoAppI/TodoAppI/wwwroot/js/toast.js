const toastSuccessElement = document.getElementById('toastSuccess');
const toastErrorElement = document.getElementById('toastError');

export const showToast = (message, isError = false) => {
    const toastElement = isError ? toastErrorElement : toastSuccessElement;
    const toastBody = toastElement.querySelector('.toast-body');
    toastBody.textContent = message;
    const toast = bootstrap.Toast.getOrCreateInstance(toastElement);
    toast.show();
};
