export interface ErrorResponse {
    error: string;
}

export interface Item {
    id: number;
    name: string;
    quantity: number;
    isChecked: boolean;
    createdAt: Date;
    modifiedAt: Date;
}

export interface ShoppingList {
    id: number;
    name: string;
    items: Item[];
    createdAt: Date;
    modifiedAt: Date;
}

export interface User {
    email: string;
    token: string;
}