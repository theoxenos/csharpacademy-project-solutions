import meals from '../data/meals.json' with { type: 'json' }
import express, { type Request, type Response } from 'express';

const app = express();
const port = process.env.PORT || 3000;

app.use(express.json());

// Base URL for TheMealDB API.
// Example: https://www.themealdb.com/api/json/v1/1/search.php?s=Arrabiata
const MEALDB_BASE_URL = process.env.MEALDB_BASE_URL ?? 'https://www.themealdb.com/api/json/v1';
const MEALDB_API_KEY = process.env.MEALDB_API_KEY ?? '1'; // use your key if you have one

function mealDbUrl(pathAndQuery: string) {
  const normalized = pathAndQuery.startsWith('/') ? pathAndQuery.slice(1) : pathAndQuery;
  return `${MEALDB_BASE_URL}/${MEALDB_API_KEY}/${normalized}`;
}

async function fetchMealDb<T>(pathAndQuery: string): Promise<T> {
  const url = mealDbUrl(pathAndQuery);
  const resp = await fetch(url, {
    headers: {
      Accept: 'application/json',
    },
  });

  if (!resp.ok) {
    const body = await resp.text().catch(() => '');
    throw new Error(`TheMealDB error ${resp.status} ${resp.statusText}: ${body}`);
  }

  return (await resp.json()) as T;
}

// -----------------------------
// Ingredient cache + search
// -----------------------------

type MealDbIngredient = {
  idIngredient: string | null;
  strIngredient: string | null;
  strDescription: string | null;
  strType: string | null;
};

type MealDbIngredientListResponse = {
  meals: MealDbIngredient[] | null;
};

const ingredientCache = {
  value: [] as MealDbIngredient[],
  fetchedAtMs: 0,
  ttlMs: 6 * 60 * 60 * 1000, // 6 hours
};

async function getCachedIngredients(forceRefresh = false): Promise<MealDbIngredient[]> {
  const now = Date.now();
  const isFresh = ingredientCache.value.length > 0 && now - ingredientCache.fetchedAtMs < ingredientCache.ttlMs;

  if (!forceRefresh && isFresh) return ingredientCache.value;

  const data = await fetchMealDb<MealDbIngredientListResponse>('list.php?i=list');
  const list = (data.meals ?? []).filter((x) => x?.strIngredient);

  ingredientCache.value = list;
  ingredientCache.fetchedAtMs = now;
  return ingredientCache.value;
}

function normalizeText(s: string) {
  return s.trim().toLowerCase();
}

// Custom route: list ingredients (optionally filter by q)
app.get('/api/ingredients', async (req: Request, res: Response) => {
  try {
    const qRaw = typeof req.query.q === 'string' ? req.query.q : '';
    const q = normalizeText(qRaw);

    const ingredients = await getCachedIngredients(false);

    const filtered =
      q.length === 0
        ? ingredients
        : ingredients.filter((i) => {
            const name = normalizeText(i.strIngredient ?? '');
            const desc = normalizeText(i.strDescription ?? '');
            return name.includes(q) || (desc.length > 0 && desc.includes(q));
          });

    res.json({
      count: filtered.length,
      ingredients: filtered,
      cachedAt: ingredientCache.fetchedAtMs,
      ttlMs: ingredientCache.ttlMs,
    });
  } catch (err) {
    res.status(502).json({ error: 'Failed to load ingredients from upstream.' });
  }
});

// Custom route: force refresh ingredient cache
app.post('/api/ingredients/refresh', async (_req: Request, res: Response) => {
  try {
    const ingredients = await getCachedIngredients(true);
    res.json({ ok: true, count: ingredients.length, cachedAt: ingredientCache.fetchedAtMs });
  } catch {
    res.status(502).json({ ok: false, error: 'Failed to refresh ingredients.' });
  }
});

// -----------------------------
// Wrapper routes (examples)
// -----------------------------

// Search meals by name: /api/meals/search?s=Arrabiata
app.get('/api/meals/search', async (req: Request, res: Response) => {
  try {
    const s = typeof req.query.s === 'string' ? req.query.s : '';
    const data = await fetchMealDb('search.php?s=' + encodeURIComponent(s));
    res.json(data);
  } catch {
    res.status(502).json({ error: 'Upstream search failed.' });
  }
});

// Lookup meal by id: /api/meals/lookup?i=52772
app.get('/api/meals/lookup', async (req: Request, res: Response) => {
  try {
    const i = typeof req.query.i === 'string' ? req.query.i : '';
    const data = await fetchMealDb('lookup.php?i=' + encodeURIComponent(i));
    res.json(data);
  } catch {
    res.status(502).json({ error: 'Upstream lookup failed.' });
  }
});

// Categories: /api/categories
app.get('/api/categories', async (_req: Request, res: Response) => {
  try {
    const data = await fetchMealDb('categories.php');
    res.json(data);
  } catch {
    res.status(502).json({ error: 'Upstream categories failed.' });
  }
});

// Simple health check
app.get('/health', (_req: Request, res: Response) => {
  res.json({ ok: true });
});

app.get('/meals', (req: Request, res: Response) => {
  res.json(meals);
});

app.listen(port, () => {
  console.log(`Server is running on port ${port}`);
});
