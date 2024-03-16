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
          const todoItemId = document.querySelector('#todoId').textContent;

          const method = todoItemId === '' ? 'post' : 'put';
          const init = {
              method,
              headers: {
                  'Content-Type': 'application/json'
              },
              body: JSON.stringify({id: todoItemId, name: nameElement.value, completed: completedElement.checked})
          }
          const response = await fetch(`todos/${todoItemId}`, init);
          
          if(response.status === 204) {
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
        console.log(todos);
        todos.forEach((t) => {
           const todoNode = document.createElement('li');
           todoNode.classList.add('list-group-item')
           
           const checkbox = document.createElement('input');
           checkbox.classList.add('form-check-inline')
           checkbox.type = 'checkbox';
           checkbox.checked = t.completed;
           
           checkbox.addEventListener('click', async (event) => {
               const init = { 
                   method: 'put',
                   headers: {
                       'Content-Type' : 'application/json'
                   },
                   body: JSON.stringify({id: t.id, completed: event.target.checked})
               };
               const response = await fetch(`/todos/${t.id}`, init);
               
               // TODO 
               if(response.status === 204) {
                   const toastBootstrap = bootstrap.Toast.getOrCreateInstance(toastLiveExample)
                   const toastBody = document.querySelector('#toastSuccess .toast-body');
                   toastBody.textContent = `${t.name} updated`
                   toastBootstrap.show();
               }
           });
           
           todoNode.appendChild(checkbox);

            const textNode = document.createTextNode(t.name);
            todoNode.appendChild(textNode);
           
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
