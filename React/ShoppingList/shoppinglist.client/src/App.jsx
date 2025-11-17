import ShoppingListsList from "./components/ShoppingListsList.jsx";
import {useDispatch} from "react-redux";
import {useEffect} from "react";
import {initialiseShoppingLists} from "./reducers/shoppingListReducer.js";

const App = () => {
    const dispatch = useDispatch();
    
    useEffect(() => {
            dispatch(initialiseShoppingLists());
        }
        , [dispatch]
    );
    
    return (
        <div className="container py-4">
            <h1 className="display-5 fw-semibold">Shopping List</h1>
            <ShoppingListsList/>
        </div>
    )
};

export default App;