import type {IngredientResponse} from "../Types.ts";

const convertIngredient = (ingredient: IngredientResponse) => ({
    Id: ingredient.idIngredient,
    Name: ingredient.strIngredient,
    Description: ingredient.strDescription ?? "",
    ThumbnailUrl: ingredient.strThumb,
    Type: ingredient.strType ?? "",
    Calories: 0,
    Carbohydrates: 0,
    Fat: 0,
    Protein: 0,
})