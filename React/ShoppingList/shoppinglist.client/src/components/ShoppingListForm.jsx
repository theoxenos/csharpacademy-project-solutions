import Card from "react-bootstrap/Card";
import {useDispatch} from "react-redux";
import {updateShoppingList} from "../reducers/shoppingListReducer.js";

const ShoppingListForm = ({list}) => {
    const dispatch = useDispatch();
    
    const handleNameChange = (e) => {
        dispatch(updateShoppingList({shoppingListId: list.id, updatedShoppingList: {...list, name: e.target.value}}))
    }
    
    return <>
        <Card>
            <Card.Header>
                {/* On change update state, on blur update backend*/}
                <input type="text" className="form-control-plaintext" value={list?.name} onChange={(e) => list.name = e.target.value} onBlur={handleNameChange}/>
            </Card.Header>
            <Card.Body>
                {list.items.map(item => (
                    <label>
                        <input type="checkbox" checked={item.isChecked} onChange={() => {}}/>
                        {item.name}
                    </label>
                ))}
            </Card.Body>
        </Card>
    </>
};

export default ShoppingListForm;