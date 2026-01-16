const $transactionsTableRows = $('#transactionsTableRows');
const $newTransactionButton = $('#btnTransactionNew');
const $upsertModalElement = $('#transactionUpsertModal');
const upsertModal = new bootstrap.Modal($upsertModalElement[0]);
const $submitUpsertModalButton = $('#btnTransactionUpsert');
const $upsertForm = $('#frmTransactionUpsert');
const $confirmationModalElement = $('#confirmationModal');
const confirmationModal = new bootstrap.Modal($confirmationModalElement[0]);
const $transactionDeleteIdElement = $('#deleteTransactionId');
const $confirmDeleteButton = $('#btnTransactionConfirmDelete');

$upsertForm.on('submit', function (event) {
    event.preventDefault();

    if (!$upsertForm.valid()) {
        return;
    }

    const transaction = {
        id: Number($upsertForm.find('[name="Transaction.Id"]').val()),
        date: $upsertForm.find('[name="Transaction.Date"]').val(),
        comment: $upsertForm.find('[name="Transaction.Comment"]').val(),
        amount: $upsertForm.find('[name="Transaction.Amount"]').val(),
        categoryId: $upsertForm.find('[name="Transaction.CategoryId"]').val()
    }

    let method = 'PUT';
    let route = `/transactions/update/${transaction.id}`;
    if (transaction.id === 0) {
        method = 'POST';
        route = '/transactions/create';
    }

    $.ajax({
        url: route,
        method: method,
        contentType: 'application/json',
        data: JSON.stringify(transaction),
        success: function (data) {
            $transactionsTableRows.html(data);
            upsertModal.hide();
        },
        error: function (xhr) {
            if (xhr.status !== 400) {
                console.error(`HTML Error: ${xhr.status}`);
            }
        }
    });
});

$submitUpsertModalButton.click(() => {
    $upsertForm.trigger('submit');
});

$transactionsTableRows.on('click', '.edit-transaction', function () {
    if (typeof resetFormValidation === 'function') {
        resetFormValidation($upsertForm);
    }

    const transactionId = $(this).data('transactionid');
    $.get('Transactions/Detail/' + transactionId, function (transaction) {
        $upsertForm.find('[name="Transaction.Id"]').val(transaction.id);
        if (transaction.date) {
            $upsertForm.find('[name="Transaction.Date"]').val(transaction.date.substring(0, 16));
        }
        $upsertForm.find('[name="Transaction.Comment"]').val(transaction.comment);
        $upsertForm.find('[name="Transaction.Amount"]').val(transaction.amount);
        $upsertForm.find('[name="Transaction.CategoryId"]').val(transaction.categoryId);

        upsertModal.show();
    }).fail(function (xhr) {
        console.error(`HTML Error: ${xhr.status}`);
    });
});

$transactionsTableRows.on('click', '.delete-transaction', function () {
    $transactionDeleteIdElement.val($(this).data('transactionid'));
    confirmationModal.show();
});

$confirmDeleteButton.click(function () {
    const transactionId = Number($transactionDeleteIdElement.val());
    $.ajax({
        url: `Transactions/Delete/${transactionId}`,
        method: 'DELETE',
        success: function (data) {
            confirmationModal.hide();
            $transactionsTableRows.html(data);
        },
        error: function (xhr) {
            console.error(`HTML Error: ${xhr.status}`);
        }
    });
});

$newTransactionButton.click(() => {
    if (typeof resetFormValidation === 'function') {
        resetFormValidation($upsertForm);
    }
    
    $upsertForm[0].reset();
    $upsertForm.find('[name="Transaction.Id"]').val(0);

    upsertModal.show();
});

const $searchForm = $('#frmSearch');
if ($searchForm.length) {
    $searchForm.submit(function (e) {
        e.preventDefault();

        const searchVm = {
            CategoryFilter: $searchForm.find('[name="CategoryFilter"]').val() || 0,
            DateFilter: $searchForm.find('[name="DateFilter"]').val() || '',
            TransactionFilter: $searchForm.find('[name="TransactionFilter"]').val() || ''
        };

        const queryString = $.param(Object.fromEntries(
            Object.entries(searchVm).filter(([_, v]) => v !== null && v !== undefined && v !== '' && v !== 0 && v !== '0')
        ));

        $.get(`/Transactions/Filter/?${queryString}`, function (data) {
            $transactionsTableRows.html(data);
        });
    });

    $searchForm.find('button[type="button"]').click(function () {
        $searchForm[0].reset();
        $searchForm.trigger('submit');
    });
}