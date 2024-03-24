window.closeBootstrapModal = function(modalId) {
    bootstrap.Modal.getInstance(document.getElementById(modalId)).hide();
}