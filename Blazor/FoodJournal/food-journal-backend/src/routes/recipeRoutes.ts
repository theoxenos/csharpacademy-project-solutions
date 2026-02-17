import { Router } from 'express';

const router = Router();

router.get('/', async (req, res) => {
    const mealApiResponse = await fetch('https://www.themealdb.com/api/json/v1/1/search.php?s=Arrabiata');
    
    if(!mealApiResponse.ok) {
        res.status(mealApiResponse.status).send(mealApiResponse.statusText);
        return;
    }
    
    const meals = await mealApiResponse.json();
    res.json(meals);
})

router.get('/search', async (req, res) => {
    const { name } = req.query;

    if (!name) {
        res.status(400).send('Query parameter "name" is required');
        return;
    }

    const mealApiResponse = await fetch(`https://www.themealdb.com/api/json/v1/1/search.php?s=${encodeURIComponent(name as string)}`);

    if (!mealApiResponse.ok) {
        res.status(mealApiResponse.status).send(mealApiResponse.statusText);
        return;
    }

    const meals = await mealApiResponse.json();
    res.json(meals);
})

router.get('/:id', async (req, res) => {
    const { id } = req.params;

    const mealApiResponse = await fetch(`https://www.themealdb.com/api/json/v1/1/lookup.php?i=${encodeURIComponent(id)}`);

    if (!mealApiResponse.ok) {
        res.status(mealApiResponse.status).send(mealApiResponse.statusText);
        return;
    }

    const meal = await mealApiResponse.json();
    res.json(meal);
})

export default router;