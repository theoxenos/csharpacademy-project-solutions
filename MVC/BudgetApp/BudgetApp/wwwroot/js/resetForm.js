function resetFormValidation(formSelector) {
    const $form = $(formSelector);

    // Remove error class from input elements
    $form.find('.input-validation-error').removeClass('input-validation-error');

    // Hide validation message elements and reset their state
    $form.find('.field-validation-error')
        .removeClass('field-validation-error')
        .addClass('field-validation-valid')
        .empty();

    // Reset the form's validation state
    const validator = $form.validate();
    if (validator) {
        validator.resetForm();
    }
}