import app from './app.js';
import {connectDB} from "./config/database.js";

const PORT = process.env.PORT || 3000;

(async () => {
    await connectDB();
    
    await app.listen(PORT, () => console.log(`🚀 Server listening on ${PORT}`));
})();