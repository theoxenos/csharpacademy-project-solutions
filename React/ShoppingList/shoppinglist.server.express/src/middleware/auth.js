export const requireAuth = (req, res, next) => {
    const authHeader = req.headers.authorization;
    
    if(!authHeader?.startsWith('Bearer ')) {
        return res.status(401).send('Not authorized');
    }
}