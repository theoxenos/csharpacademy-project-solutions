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

export type CallbackWithId = ((id: number) => void);
export type Callback = (() => void);