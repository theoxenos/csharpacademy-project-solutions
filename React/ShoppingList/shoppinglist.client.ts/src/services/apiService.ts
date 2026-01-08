 import store from "../store";
 import type {User} from "../types.ts";

const request = async <T>(endpoint: string, options = {} as RequestInit) => {
    const state = store.getState();
    const token = (state.user as User).token;
    
    const headers = new Headers(options.headers);
    if (token) {
        headers.set('Authorization', `Bearer ${token}`);
    }

    const response = await fetch(endpoint, {
        ...options,
        headers: headers
    });
    
    if(response.headers.get('content-type') === 'application/json') {
        return await response.json() as T;
    }
    
    return null;
};

export const get = async (endpoint: string, options = {}) => {
    return await request(endpoint, options);
};