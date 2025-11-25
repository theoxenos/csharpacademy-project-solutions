import {Router} from "express";
import {createShoppingListItem, updateShoppingListItem} from "../controllers/shoppinglistItems.controller.js";

const router = Router();

router.post('/', createShoppingListItem);
router.put('/:id', updateShoppingListItem);

export default router;