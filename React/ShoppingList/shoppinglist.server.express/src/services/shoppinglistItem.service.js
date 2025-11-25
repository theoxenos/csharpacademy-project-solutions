import ShoppingListItem from "../models/shoppinglistItem.model.js";
import ShoppingList from "../models/shoppinglist.model.js";

export const createShoppingListItem = async (shoppingListItem) => {
    const now = new Date().toISOString();

    const newShoppingListItem = await ShoppingListItem.create({
        name: shoppingListItem.name,
        quantity: 0,
        isChecked: false,
        createdAt: now,
        shoppingList: shoppingListItem.shoppingListId,
    });

    return newShoppingListItem;
};

export const updateShoppingListItem = async (toUpdateShoppingListItem) => {
    const databaseShoppingListItem = await ShoppingListItem.findById(
        toUpdateShoppingListItem.id
    );

    if (!databaseShoppingListItem) {
        throw new Error(`Shopping list item ${toUpdateShoppingListItem.shoppingListId} not found`);
    }

    // Moved to different list
    if (databaseShoppingListItem.shoppingList.toString() !== toUpdateShoppingListItem.shoppingListId) {
        console.log('no matching shoppinglist: ' + toUpdateShoppingListItem.id);
        // Remove from old shopping list
        await ShoppingList.findByIdAndUpdate(databaseShoppingListItem.shoppingList,
            {$pull: {items: toUpdateShoppingListItem.id}}
        );

        // Add to new shopping list
        await ShoppingList.findByIdAndUpdate(toUpdateShoppingListItem.shoppingListId,
            {$addToSet: {items: toUpdateShoppingListItem.id}}
        );
    }

    return ShoppingListItem.findByIdAndUpdate(toUpdateShoppingListItem.id, {
        name: toUpdateShoppingListItem.name,
        quantity: toUpdateShoppingListItem.quantity,
        isChecked: toUpdateShoppingListItem.isChecked,
        shoppingList: toUpdateShoppingListItem.shoppingListId,
    }, {new: true});
};