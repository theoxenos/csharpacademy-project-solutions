import {useDispatch, useSelector} from "react-redux";
import {toggleItemChecked} from "../reducers/shoppingListReducer.js";

const ShoppingListItem = ({listItem}) => {


    return (
        <div style={itemStyle}>
            {listItem.name}
        </div>
    );
};

const shoppingList = () => {
    const dispatch = useDispatch();

    const shoppingLists = useSelector(state => state.shoppingLists);

    const handleCheckedChange = (itemId) => {
        dispatch(toggleItemChecked({
            shoppingListId: list.id,
            itemId: item.id
        }))
    };

    return <>
        {shoppingLists.map(list => <div className="card h-100">
            <div className="card-header">{list.name}</div>
            <div className="card-body">
                <ul className="list-group list-group-flush">
                    {list.items.map(item => (
                        <li key={item.id} className="list-group-item">
                            <input className="me-1"
                                   type="checkbox"
                                   checked={item.isChecked}
                                   onChange={handleCheckedChange}
                            />
                            {item.name}
                        </li>))}
                </ul>
            </div>
        </div>)}
    </>
}

export default shoppingList;