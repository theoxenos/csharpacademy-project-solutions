import mongoose from "mongoose";

const shoppingListSchema = new mongoose.Schema({
    name: String,
    createdAt: String,
    modifiedAt: String,
    items: [
        {
            type: mongoose.Schema.Types.ObjectId,
            ref: "Item",
        }
    ],
}, {
    timestamps: true,
});

export default mongoose.model('ShoppingList', shoppingListSchema);