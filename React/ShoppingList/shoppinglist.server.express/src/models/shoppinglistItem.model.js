import mongoose from "mongoose";

const shoppinglistItemSchema = new mongoose.Schema({
    name: String,
    quantity: Number,
    isChecked: Boolean,
    createdAt: String,
    shoppingList: {
        type: mongoose.Schema.Types.ObjectId,
        ref: 'ShoppingList',
    }
}, {
    timestamps: true,
});

shoppinglistItemSchema.set('toJSON', {
    transform: (doc, ret) => {
        const {_id, __v, updatedAt, ...cleanShoppingListItem} = ret;
        
        return {
            ...cleanShoppingListItem,
            id: _id.toString(),
            modifiedAt: updatedAt,
        };
    }
});

export default mongoose.model("ShoppingListItem", shoppinglistItemSchema);