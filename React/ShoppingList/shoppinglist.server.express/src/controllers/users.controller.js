import * as userService from '../services/user.service.js';
import jwt from "jsonwebtoken";

const JWT_SECRET = process.env.JWT_SECRET || 'mysupersecretsecretdonotcommit';
const TOKEN_EXPIRATION = '1h';

const generateAuthToken = (user) => {
    const userForToken = {
        email: user.email,
        id: user._id
    };
    return jwt.sign(userForToken, JWT_SECRET, {expiresIn: TOKEN_EXPIRATION});
};

export const login = async (req, res, next) => {
    const {email, password} = req.body;

    if (!email || !password) {
        return res.status(400).send({error: 'Email and password are required'});
    }

    let user = await userService.getUserByEmail(email);
    if (!user) {
        user = await userService.createUser(email, password);
    }

    const isPasswordCorrect = await userService.checkPasswordMatch(email, password);
    if (!isPasswordCorrect) {
        return res.status(401).send({error: 'Password is incorrect'});
    }

    const token = generateAuthToken(user);

    return res.json({token, email});
};