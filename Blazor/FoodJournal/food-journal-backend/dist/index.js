import meals from '../data/meals.json' with { type: 'json' };
import express, {} from 'express';
import * as assert from "node:assert";
const app = express();
const port = process.env.PORT || 3000;
app.use(express.json());
app.get('/', (req, res) => {
    res.json(meals);
});
app.listen(port, () => {
    console.log(`Server is running on port ${port}`);
});
//# sourceMappingURL=index.js.map