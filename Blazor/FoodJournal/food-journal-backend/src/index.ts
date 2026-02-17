import express from 'express';
import ingredientsRouter from './routes/ingredientsRoutes.ts';
import recipeRouter from './routes/recipeRoutes.ts';

const app = express();
const port = process.env.PORT || 3000;

app.use(express.json());

app.use('/ingredients', ingredientsRouter)
app.use('/recipes', recipeRouter)

app.listen(port, () => {
  console.log(`Server is running on port ${port}`);
});
