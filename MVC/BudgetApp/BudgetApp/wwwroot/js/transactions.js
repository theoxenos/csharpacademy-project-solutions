// Dynamic content
const transactionsTableRows = document.querySelector('#transactionsTableRows');

// Table buttons
const newTransactionButton = document.querySelector('#btnTransactionNew');

// Transaction modal elements
const upsertModalElement = document.querySelector('#transactionUpsertModal');

const upsertModal = new bootstrap.Modal(upsertModalElement);
const submitUpsertModalButton = document.querySelector('#btnTransactionUpsert');
const upsertForm = document.querySelector('#frmTransactionUpsert');

// Confirm modal elements
const confirmationModalElement = document.querySelector('#confirmationModal');

const confirmationModal = new bootstrap.Modal(confirmationModalElement);
const transactionDeleteIdElement = document.querySelector('#deleteTransactionId');
const confirmDeleteButton = document.querySelector('#btnTransactionConfirmDelete');

upsertForm.addEventListener('submit', async (event) => {
    event.preventDefault();

    const formData = new FormData(event.target);
    const transaction = {
        id: Number(formData.get('Transaction.Id')),
        date: formData.get('Transaction.Date'),
        comment: formData.get('Transaction.Comment'),
        amount: formData.get('Transaction.Amount'),
        categoryId: formData.get('Transaction.CategoryId')
    }

    let method = 'PUT';
    let route = `/transactions/update/${transaction.id}`;
    if (transaction.id === 0) {
        method = 'POST';
        route = '/transactions/create';
    }

    const init = {
        method,
        headers: {
            'Content-Type' : 'application/json'
        },
        body: JSON.stringify(transaction)
    }
    const response = await fetch(route, init);

    if (response.status === 400) {
        // upsertForm.innerHTML = await response.text();
        return;
    } else if (!response.ok) {
        throw new Error(`HTML Error: ${response.status}`);
    }

    transactionsTableRows.innerHTML = await response.text();
    upsertModal.hide();
});

submitUpsertModalButton.addEventListener('click', () => {
    // Create a new 'submit' event
    let event = new Event('submit', {
        bubbles: true, // Event will bubble up through the DOM
        cancelable: true // Event can be canceled
    });

    // Dispatch it on the form
    if (upsertForm.dispatchEvent(event)) {
        upsertModal.hide(); // Hide the modal only if the event wasn't canceled
    }
});

transactionsTableRows.addEventListener('click', async (event) => {
    if (!event.target) {
        return;
    }

    if (event.target.matches('.edit-transaction')) {
        if (typeof resetFormValidation === 'function') {
            resetFormValidation(upsertForm);
        }

        const transactionId = event.target.dataset.transactionid;
        const response = await fetch('Transactions/Detail/' + transactionId);

        if (!response.ok) {
            throw new Error(`HTML Error: ${response.status}`);
        }

        const transaction = await response.json();

        upsertForm['Transaction.Id'].value = transaction.id;
        upsertForm['Transaction.Date'].value = transaction.date;
        upsertForm['Transaction.Comment'].value = transaction.comment;
        upsertForm['Transaction.Amount'].value = transaction.amount;
        upsertForm['Transaction.CategoryId'].value = transaction.categoryId;

        upsertModal.show();
    } else if (event.target.matches('.delete-transaction')) {
        transactionDeleteIdElement.value = event.target.dataset.transactionid;
        confirmationModal.show();
    }
});

confirmDeleteButton.addEventListener('click', async () => {
    const transactionId = Number(transactionDeleteIdElement.value);
    const response = await fetch(`Transactions/Delete/${transactionId}`, { method: 'delete'})

    if (!response.ok) {
        throw new Error(`HTML Error: ${response.status}`);
    }

    confirmationModal.hide();
    transactionsTableRows.innerHTML = await response.text();
});

newTransactionButton.addEventListener('click', () =>{
    if (typeof resetFormValidation === 'function') {
        resetFormValidation(upsertForm);
    }

    upsertForm['Transaction.Id'].value = '';
    upsertForm['Transaction.Date'].value = '';
    upsertForm['Transaction.Comment'].value = '';
    upsertForm['Transaction.Amount'].value = '';
    upsertForm['Transaction.CategoryId'].value = '0';

    upsertModal.show();
});

const searchForm = document.querySelector('#frmSearch');
if (searchForm) {
    searchForm.addEventListener('submit', async (e) => {
        e.preventDefault();

        const formData = new FormData(e.target);

        const searchVm = {
            CategoryFilter: formData.get('CategoryFilter'),
            DateFilter: formData.get('DateFilter'),
            TransactionFilter: formData.get('TransactionFilter')
        };

        const queryString = Object.keys(searchVm).map(key =>
            `${encodeURIComponent(key)}=${encodeURIComponent(searchVm[key])}`
        ).join('&');

        const url = `Transactions/Filter/?${queryString}`;
        const response = await fetch(url);

        transactionsTableRows.innerHTML = await response.text();
    });

    const clearButton = searchForm.querySelector('button[type="button"]');
    if (clearButton) {
        clearButton.onclick = null; // Remove inline handler
        clearButton.addEventListener('click', () => {
            searchForm.reset();
            searchForm.dispatchEvent(new Event('submit'));
        });
    }
}