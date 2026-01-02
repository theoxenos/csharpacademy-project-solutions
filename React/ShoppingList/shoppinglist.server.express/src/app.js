import express from 'express';
import cors from 'cors';
import helmet from 'helmet';
import morgan from 'morgan';
import routes from './routes/index.js';
import {requireAuth} from './middleware/auth.js';
// import { errorHandler } from './middleware/errorHandler.js';

const app = express();

app.use(cors());
app.use(helmet());
app.use(express.json());
app.use(morgan('dev'));

app.use(requireAuth);
app.use('/api', routes);          // mount all routers under /api
// app.use(errorHandler);           // catch‑all error middleware

export default app;