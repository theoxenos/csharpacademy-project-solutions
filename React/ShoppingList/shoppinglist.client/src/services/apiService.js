 import store from "../store.js";

const request = async (endpoint, options = {}) => {
    const state = store.getState();
    const token = state.user.token;

    const response = await fetch(endpoint, {
        ...options,
        headers: {
            Authorization: `Bearer ${token}`,
            ...options.headers,
        }
    });
    
    if(response.headers['content-type'] === 'application/json') {
        return await response.json();
    }
    
    return null;
};

export const get = async (endpoint, options = {}) => {
    return await request(endpoint, options);
};