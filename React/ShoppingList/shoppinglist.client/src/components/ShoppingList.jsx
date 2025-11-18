import {useDispatch} from "react-redux";
import {toggleItemChecked} from "../reducers/shoppingListReducer.js";

const ShoppingListItem = ({listItem, onCheckedChange}) => {
    const handleCheckedChange = () => {
        onCheckedChange(listItem.id);
    };

    return (
        <li className="list-group-item">
            <label>
                <input className="me-1"
                       type="checkbox"
                       checked={listItem.isChecked}
                       onChange={handleCheckedChange}
                />
                {listItem.name}
            </label>
        </li>
    );
};

const shoppingList = ({list}) => {
    const dispatch = useDispatch();

    const handleCheckedChange = (itemId) => {
        dispatch(toggleItemChecked({
            shoppingListId: list.id,
            itemId: itemId
        }));
    };

    return <>
        <div className="card h-100">
            <div className="card-header">{list.name}</div>
            <div className="card-body">
                <ul className="list-group list-group-flush">
                    {list.items.length > 0 && list.items.map(item => (
                        <ShoppingListItem key={item.id} listItem={item} onCheckedChange={handleCheckedChange}/>))}
                </ul>
            </div>
        </div>
    </>;
};

export default shoppingList;