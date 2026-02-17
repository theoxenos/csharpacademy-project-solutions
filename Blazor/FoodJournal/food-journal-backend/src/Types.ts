export interface Ingredient {
    Id: number;
    Name: string;
    Description: string;
    ThumbnailUrl: string;
    Type: string;
    Calories: number;
    Carbohydrates: number;
    Fat: number;
    Protein: number;
}

export interface IngredientResponse {
    idIngredient:   string;
    strIngredient:  string;
    strDescription: null | string;
    strThumb:       string;
    strType:        null | string;
}