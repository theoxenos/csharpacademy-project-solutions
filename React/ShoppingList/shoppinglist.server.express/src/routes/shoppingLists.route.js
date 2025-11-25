import {Router} from "express";
import {addShoppingList, getAllShoppingLists, deleteShoppingList, updateShoppingList} from "../controllers/shoppinglists.controller.js";

const router  = Router();

router.get("/", getAllShoppingLists);
router.post("/", addShoppingList);
router.delete("/:id", deleteShoppingList);
router.put("/:id", updateShoppingList);

export default router ;