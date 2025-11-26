import User from "../models/user.model.js";
import bcrypt from "bcrypt";

export const getUserByEmail = async (email) => {
    return User.findOne({email});
};

export const createUser = async (email, password) => {
    const saltRounds = 10;
    const passwordHash = bcrypt.hashSync(password, saltRounds);
    
    return await User.create({
        email,
        passwordHash,
    });
};

export const checkPasswordMatch = async (email, password) => {
    const user = await User.findOne({email});
    
    return await bcrypt.compare(password, user.passwordHash);
};