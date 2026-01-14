const modal = new bootstrap.Modal('#deleteModal');

let onModalSubmitted = () => {};
const setOnModalSubmitted = (callback) => onModalSubmitted = callback;

const show = () => modal.show();

const handleConfirm = () => {
  modal.hide();
  onModalSubmitted();
};

const init = () => {
  const confirmButton = document.getElementById('deleteModalConfirm');
  confirmButton.onclick = handleConfirm;
};

init();

export default {show, setOnModalSubmitted};