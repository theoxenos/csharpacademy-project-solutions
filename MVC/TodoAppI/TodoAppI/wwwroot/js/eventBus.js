const eventBus = {
    events: {},

    on(eventName, callback) {
        if (!this.events[eventName]) this.events[eventName] = [];
        this.events[eventName].push(callback);
    },

    off(eventName, callback) {
        if (!this.events[eventName]) return;
        this.events[eventName] = this.events[eventName].filter(cb => cb !== callback);
    },

    async emit(eventName, ...args) {
        if (!this.events[eventName]) return;
        
        const handlers = this.events[eventName].map(cb => cb(...args));
        await Promise.all(handlers);
    }
};

export default eventBus;