import mongoose from "mongoose";

const User = new mongoose.Schema({
    email: {
        type: String,
        required: true,
    },
    passwordHash: {
        type: String,
        required: true,
    },
    shoppingLists: [
        {
            type: mongoose.Schema.Types.ObjectId,
            ref: "ShoppingList",
        }
    ]
});

export default mongoose.model("User", User);