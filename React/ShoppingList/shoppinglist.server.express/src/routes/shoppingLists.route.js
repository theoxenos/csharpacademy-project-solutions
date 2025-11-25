import {Router} from "express";
import {getAllShoppingLists} from "../controllers/shoppinglists.controller.js";

const router  = Router();

router.get("/", getAllShoppingLists);

export default router ;