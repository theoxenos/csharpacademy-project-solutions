import {useDispatch} from "react-redux";
import {createShoppingList} from "../reducers/shoppingListReducer.js";

export const NewShoppingListForm = () => {
    const dispatch = useDispatch();

    const handleSubmit = (e) => {
        e.preventDefault();
        
        const shoppingListName = e.target.elements[0].value;
        dispatch(createShoppingList({name: shoppingListName}));

        e.target.reset();
    };

    return <form className="w-auto" onSubmit={handleSubmit}>
        <div className="d-flex justify-content-center">
            <div className="input-group">
                <input id="shoppinglist-name"
                       type="text"
                       placeholder="Your new list's name..."
                       className="form-control border-dark-subtle"
                />
                <button type="submit" className="btn btn-primary">
                    <i className="bi bi-check-lg"></i>
                </button>
                <button type="reset" className="btn btn-danger">
                    <i className="bi bi-x-lg"></i>
                </button>
            </div>
        </div>
    </form>;
};

export default NewShoppingListForm;