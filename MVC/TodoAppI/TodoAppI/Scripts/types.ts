export interface PagedResponse<T> {
    data: T[];
    pageNumber: number;
    pageSize: number;
    totalRecords: number;
}

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