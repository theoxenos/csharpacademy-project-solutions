import { Router } from 'express';
import shoppingListsRoutes from "./shoppingLists.route.js";
import shoppinglistItemsRoute from "./shoppinglistItems.route.js";
import userRoutes from "./users.route.js";

const router = Router();

router.use('/shoppinglists', shoppingListsRoutes);
router.use('/items', shoppinglistItemsRoute);
router.use('/users', userRoutes);

export default router;