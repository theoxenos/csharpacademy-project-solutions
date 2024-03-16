const todoModule = (() => {
    let todos = [];

    const toastLiveExample = document.getElementById('toastSuccess');
    const upsertModal = new bootstrap.Modal('#upsertModal');
    const upsertModalSubmit = document.querySelector('#upsertModalSubmit');
    
    const getElements = () => {
        this.button = document.querySelector('#btnAddTodo');
        this.todoList = document.querySelector('#todoList');
    }
    const addEventHandlers = () => {
      button.addEventListener('click', () => {
          const completedElement = document.querySelector('#todo-completed');
          const nameElement = document.querySelector('#todo-name');
          const todoItemId = document.querySelector('#todoId');
          
          completedElement.checked = false;
          nameElement.value = '';
          todoItemId.value = '';
          
          upsertModal.show();
      });
      
      upsertModalSubmit.addEventListener('click', async () => {
          const nameElement = document.querySelector('#todo-name');
          const completedElement = document.querySelector('#todo-completed');
          const todoItemId = document.querySelector('#todoId').value;

          const method = todoItemId === '' ? 'post' : 'put';
          const init = {
              method,
              headers: {
                  'Content-Type': 'application/json'
              },
              body: JSON.stringify({id: Number(todoItemId), name: nameElement.value, completed: completedElement.checked})
          }
          const response = await fetch(`todos/${todoItemId}`, init);
          
          if(response.status === 204) {
              upsertModal.hide();
              
              await fetchTodos();
              logTodos();
              
              const toastBootstrap = bootstrap.Toast.getOrCreateInstance(toastLiveExample)
              const toastBody = document.querySelector('#toastSuccess .toast-body');
              toastBody.textContent = `${nameElement.value} created`
              toastBootstrap.show();
          }
      });
    };
    
    const fetchTodos = async () => {
        const response = await fetch('/todos');
        todos = await response.json();
    };

    const logTodos = () => {
        this.todoList.innerHTML = '';
        
        todos.forEach((t) => {
           const todoNode = document.createElement('li');
           todoNode.classList.add('list-group-item', 'd-flex', 'align-items-center', 'justify-content-between');

            const checkboxGroup = document.createElement('span');
           
            const labelElement = document.createElement('label');
            labelElement.classList.add('ms-2');
            labelElement.textContent = t.name;
                        
           const checkboxElement = document.createElement('input');
           checkboxElement.classList.add('form-check-input')
           checkboxElement.type = 'checkbox';
           checkboxElement.checked = t.completed;
           
           checkboxElement.addEventListener('click', async (event) => {
               const init = { 
                   method: 'put',
                   headers: {
                       'Content-Type' : 'application/json'
                   },
                   body: JSON.stringify({id: t.id, completed: event.target.checked})
               };
               const response = await fetch(`/todos/${t.id}`, init);
               
               if(response.status === 204) {
                   const toastBootstrap = bootstrap.Toast.getOrCreateInstance(toastLiveExample)
                   const toastBody = document.querySelector('#toastSuccess .toast-body');
                   toastBody.textContent = `${t.name} updated`
                   toastBootstrap.show();
               }
           });
           
           checkboxGroup.appendChild(checkboxElement);
           checkboxGroup.appendChild(labelElement);           
           todoNode.appendChild(checkboxGroup);
           
            const buttonGroup = document.createElement('span');
            const btnUpdate = document.createElement('button');
            const btnDelete = document.createElement('button');

            btnUpdate.classList.add('btn', 'btn-warning', 'bi', 'bi-pencil');
            btnDelete.classList.add('btn', 'btn-danger',  'bi', 'bi-trash');
            
            btnUpdate.addEventListener('click', () =>{
                const nameElement = document.querySelector('#todo-name');
                const completedElement = document.querySelector('#todo-completed');
                const todoItemIdElement = document.querySelector('#todoId');
                
                todoItemIdElement.value = t.id;
                nameElement.value = t.name;
                completedElement.checked = t.completed;
                
                upsertModal.show();                
            });
            
            btnDelete.addEventListener('click', async () => {
                const response = await fetch(`/todos/${t.id}`, {method: 'delete'});

                if(response.status === 204) {
                    await fetchTodos();
                    logTodos();

                    const toastBootstrap = bootstrap.Toast.getOrCreateInstance(toastLiveExample)
                    const toastBody = document.querySelector('#toastSuccess .toast-body');
                    toastBody.textContent = `${t.name} deleted`
                    toastBootstrap.show();
                }
            });
            
            buttonGroup.append(btnUpdate, btnDelete);
            todoNode.appendChild(buttonGroup);
            
           this.todoList.appendChild(todoNode);
        });
    };

    return {
        init: async () => {
            getElements();
            addEventHandlers();
            await fetchTodos();
            logTodos();
        }
    };
})();

todoModule.init();
