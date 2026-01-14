import todosApiService from "./todosApiService.js";
import eventBus from "./eventBus.js";

export const deleteTodoItemListener = async (id) => {
    try {
        await todosApiService.deleteTodoById(id);
        void eventBus.emit('todoDeleted', id);
    } catch (error) {
        void eventBus.emit('error', error.message);
    }
}