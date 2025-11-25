import { Router } from 'express';
import shoppingListsRoutes from "./shoppingLists.route.js";
import shoppinglistItemsRoute from "./shoppinglistItems.route.js";

const router = Router();

router.use('/shoppinglists', shoppingListsRoutes);
router.use('/items', shoppinglistItemsRoute);

export default router;