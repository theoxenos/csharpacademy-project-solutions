import mongoose from "mongoose";

const shoppingListSchema = new mongoose.Schema({
    name: String,
    createdAt: String,
    items: [
        {
            type: mongoose.Schema.Types.ObjectId,
            ref: "Item",
        }
    ],
}, {
    timestamps: true,
});

shoppingListSchema.set('toJSON', {
    transform: (doc, ret) => {
        const {_id, __v, updatedAt, ...cleanShoppingList} = ret;
        
        return {
            ...cleanShoppingList,
            id: _id.toString(),
            modifiedAt: updatedAt,
        };
    }
});

export default mongoose.model('ShoppingList', shoppingListSchema);