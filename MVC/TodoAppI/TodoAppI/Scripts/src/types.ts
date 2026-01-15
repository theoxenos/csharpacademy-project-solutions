export interface Todo {
    id: number;
    name: string;
    completed: boolean;
}

export interface TodoUpsert {
    id?: number;
    name: string;
    completed: boolean;
}
