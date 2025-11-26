const loginUser = async (email, password) => {
    const response = await fetch('/api/users/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({email, password}),
    });
    
    if (!response.ok) {
        throw new Error(`${response.status} ${response.statusText}`);
    }
    
    return await response.json();
};

export default {loginUser};