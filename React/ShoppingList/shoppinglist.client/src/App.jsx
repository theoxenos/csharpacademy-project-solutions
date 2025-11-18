import {useDispatch, useSelector} from "react-redux";
import {useEffect} from "react";
import 'bootstrap-icons/font/bootstrap-icons.css';

import {initialiseShoppingLists} from "./reducers/shoppingListReducer.js";
import ShoppingList from "./components/ShoppingList.jsx";

const App = () => {
    const dispatch = useDispatch();

    useEffect(() => {
            dispatch(initialiseShoppingLists());
        }
        , [dispatch]
    );

    let shoppingLists = useSelector(state => state.shoppingLists);

    return (
        // <div className="container py-4">
        //     <h1 className="display-5 fw-semibold">Shopping List</h1>
        //     <div className="row border border-1 border-dark rounded">
        //         <div className="col-md-3 border border-1 border-dark border-start-0 border-top-0 border-bottom-0">
        //             <ShoppingListsList/>
        //         </div>
        //         <div className="col">
        //            
        //         </div>
        //     </div>
        // </div>
        <div className="container-xxl py-3">
            <div className="mb-3">
                <h1 className="display-6 text-center">
                    <span className="text-success">
                        <i className="bi bi-cart"></i>
                    </span>
                    Your Shopping Lists
                </h1>
            </div>
            <div className="row row-cols-2 row-cols-lg-3 g-3">
                {shoppingLists.map(list => (
                    <div key={list.id} className="col">
                        <ShoppingList list={list}/>
                    </div>
                ))}
            </div>
        </div>
    )
};

export default App;