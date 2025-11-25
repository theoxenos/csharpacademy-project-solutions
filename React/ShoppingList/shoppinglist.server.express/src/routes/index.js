import { Router } from 'express';
import shoppingListsRoutes from "./shoppingLists.route.js";

const router = Router();

router.use('/shoppinglists', shoppingListsRoutes);


export default router;