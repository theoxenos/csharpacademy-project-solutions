import {useDispatch, useSelector} from "react-redux";
import {useEffect} from "react";
import {initialiseShoppingLists, toggleItemChecked} from "./reducers/shoppingListReducer.js";

const App = () => {
    const dispatch = useDispatch();

    useEffect(() => {
            dispatch(initialiseShoppingLists());
        }
        , [dispatch]
    );

    let shoppingLists = useSelector(state => state.shoppingLists);

    const handleClick = (event) => {}
    
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
            <div>
                <h1 className="display-6">
            <span className="text-danger">
                <i className="bi bi-fire"></i>
            </span>
                    Popular Movies
                </h1>
            </div>
            {/*flex-md-row removed*/}
            <div className="d-flex flex-column  justify-content-between my-3">

                <div
                    className="row row-cols-2 row-cols-lg-4 row-cols-xl-5 g-3 justify-content-center justify-content-md-start">
                    {shoppingLists.map(list => (<div key={list.id} className="col">
                        
                    </div>))}
                </div>

            </div>
        </div>
    )
};

export default App;