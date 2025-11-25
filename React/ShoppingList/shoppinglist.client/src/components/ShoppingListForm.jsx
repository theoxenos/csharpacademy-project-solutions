import Card from "react-bootstrap/Card";
import {useDispatch} from "react-redux";
import {createItem, updateShoppingList, updateShoppingListItem} from "../reducers/shoppingListReducer.js";
import ListGroup from "react-bootstrap/ListGroup";
import shoppingListService from "../services/shoppingListService.js";
import {useEffect, useRef} from "react";

const ShoppingListItemInput = ({item, onCheckedChange, onNameChange, onQuantityChange, inputRef}) => {
    const handleCheckedChange = () => {
        onCheckedChange(item.id);
    };
    const handleNameChange = (e) => {
        onNameChange(item.id, e.target.value);
    };
    const handleQuantityChange = (e) => {
        if (isNaN(e.target.value)) return;

        onQuantityChange(item.id, e.target.value);
    };

    return (
        <ListGroup.Item>
            <input type="checkbox" checked={item.isChecked} onChange={handleCheckedChange}/>
            <input type="text"
                   ref={inputRef}
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
    const lastItemRef = useRef(null);
    const listNameInputRef = useRef(null);

    useEffect(() => {
        if (lastItemRef.current) {
            lastItemRef.current.focus();
        }
    }, [list?.items.length]);

    useEffect(() => {
        if (listNameInputRef.current) {
            listNameInputRef.current.focus();
        }
    }, [list?.id]);


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
    };
    const handleNameBlur = async (e) => {
        await shoppingListService.updateShoppingList(list.id, e.target.value);
    };
    const handleNameChange = (e) => {
        dispatch(updateShoppingList({shoppingListId: list.id, updatedShoppingList: {...list, name: e.target.value}}));
    };
    const handleNewItemClick = (e) => {
        e.preventDefault();

        dispatch(createItem(list.id, ''));
    };

    const shoppingListItemsSorted = [...list?.items || []].sort((a, b) => a.createdAt.localeCompare(b.createdAt));

    return <>
        {list &&
            <Card>
                <Card.Header>
                    <input type="text"
                           ref={listNameInputRef}
                           className="form-control-plaintext"
                           value={list?.name}
                           onChange={handleNameChange}
                           onBlur={handleNameBlur}/>
                </Card.Header>
                <Card.Body>
                    <ListGroup variant="flush">
                        {shoppingListItemsSorted.map((item, index) => <ShoppingListItemInput key={item.id}
                                                                                             item={item}
                                                                                             onCheckedChange={handleItemCheckedChange}
                                                                                             onNameChange={handleItemNameChange}
                                                                                             onQuantityChange={handleItemQuantityChange}
                                                                                             inputRef={index === list.items.length - 1 ? lastItemRef : null}/>
                        )}
                        <ListGroup.Item>
                            <a href="#" className="btn" onClick={handleNewItemClick}>
                                <i className="bi bi-plus"></i>
                                Add new item
                            </a>
                        </ListGroup.Item>
                    </ListGroup>
                </Card.Body>
            </Card>
        }
    </>;
};

export default ShoppingListForm;