function resetFormValidation(formSelector) {
    // Obtain a reference to the form element
    const $form = $(formSelector);

    // Remove error class from input elements
    $form.find('.input-validation-error').removeClass('input-validation-error');

    // Hide validation message elements
    $form.find('.field-validation-error').removeClass('field-validation-error').addClass('field-validation-valid');

    // Alternatively, you could remove the error messages if they were dynamically added
    $form.find('[id*=-error]').remove();

    // Reset the form's validation state
    $form.validate().resetForm(); // This clears the validation state

    // If you also want to reset the form fields to their default values
    // $form[0].reset();
}