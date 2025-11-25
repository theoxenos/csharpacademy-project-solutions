import mongoose from 'mongoose';
import dotenv from 'dotenv';

dotenv.config();                       // loads .env variables

const MONGODB_URI = process.env.MONGODB_URI || 'mongodb://localhost:27017/shoppingListDb';

export const connectDB = async () => {
    try {
        await mongoose.connect(MONGODB_URI, {
            // useNewUrlParser: true,
            // useUnifiedTopology: true,
            // optional: useCreateIndex, useFindAndModify are deprecated in newer versions
        });
        console.log('✅ MongoDB connected');
    } catch (err) {
        console.error('❌ MongoDB connection error:', err);
        process.exit(1);                  // crash the process – better than running without DB
    }
};