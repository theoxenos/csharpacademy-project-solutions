import type {User} from "../types.ts";

const loginUser = async (email: string, password: string) => {
    const response = await fetch('/api/users/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({email, password}),
    });
    
    if (!response.ok) {
        throw new Error(`${response.status.toString()} ${response.statusText}`);
    }
    
    return await response.json() as User;
};

export default {loginUser};