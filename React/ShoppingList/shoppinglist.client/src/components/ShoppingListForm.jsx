import Card from "react-bootstrap/Card";
import {useDispatch} from "react-redux";
import {updateShoppingList, updateShoppingListItem} from "../reducers/shoppingListReducer.js";
import ListGroup from "react-bootstrap/ListGroup";
import shoppingListService from "../services/shoppingListService.js";

const ShoppingListItemInput = ({item, onCheckedChange, onNameChange, onQuantityChange}) => {
    const handleCheckedChange = () => {
        onCheckedChange(item.id);
    }
    const handleNameChange = (e) => {
        onNameChange(item.id, e.target.value);
    }
    const handleQuantityChange = (e) => {
        if(isNaN(e.target.value)) return;
        
        onQuantityChange(item.id, e.target.value);
    }

    return (
        <ListGroup.Item>
            <input type="checkbox" checked={item.isChecked} onChange={handleCheckedChange}/>
            <input type="text"
                   className="form-control-plaintext d-inline w-auto ms-1"
                   value={item.name}
                   onChange={handleNameChange}/>
            <input type="text"
                   className="form-control-plaintext d-inline w-auto ms-1"
                   value={item.quantity}
                   onChange={handleQuantityChange}/>
        </ListGroup.Item>
    );
};

const ShoppingListForm = ({list}) => {
    const dispatch = useDispatch();

    const handleItemCheckedChange = (itemId) => {
        const item = list.items.find(item => item.id === itemId);
        dispatch(updateShoppingListItem(list.id, {...item, isChecked: !item.isChecked}));
    };
    const handleItemNameChange = (itemId, newName) => {
        const item = list.items.find(item => item.id === itemId);
        dispatch(updateShoppingListItem(list.id, {...item, name: newName}));
    };
    const handleItemQuantityChange = (itemId, newQuantity) => {
        const item = list.items.find(item => item.id === itemId);
        dispatch(updateShoppingListItem(list.id, {...item, quantity: newQuantity}));
    }
    const handleNameBlur = async (e) => {
        await shoppingListService.updateShoppingList(list.id, e.target.value);
    };
    const handleNameChange = (e) => {
        dispatch(updateShoppingList({shoppingListId: list.id, updatedShoppingList: {...list, name: e.target.value}}));
    };

    return <>
        {list && 
            <Card>
                <Card.Header>
                    <input type="text"
                           className="form-control-plaintext"
                           value={list?.name}
                           onChange={handleNameChange}
                           onBlur={handleNameBlur}/>
                </Card.Header>
                <Card.Body>
                    <ListGroup variant="flush">
                        {list?.items.map(item => <ShoppingListItemInput key={item.id}
                                                                        item={item}
                                                                        onCheckedChange={handleItemCheckedChange}
                                                                        onNameChange={handleItemNameChange}
                                                                        onQuantityChange={handleItemQuantityChange}/>
                        )}
                    </ListGroup>
                </Card.Body>
            </Card>
        }
    </>
};

export default ShoppingListForm;