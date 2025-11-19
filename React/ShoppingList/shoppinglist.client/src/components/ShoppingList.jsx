import {useDispatch} from "react-redux";
import {updateShoppingListItem} from "../reducers/shoppingListReducer.js";
import {selectList} from "../reducers/uiReducer.js";
import Card from 'react-bootstrap/Card';
import ListGroup from 'react-bootstrap/ListGroup';

const ShoppingListItem = ({listItem, onCheckedChange}) => {
    const handleCheckedChange = () => {
        onCheckedChange(listItem.id);
    };

    return (
        <ListGroup.Item>
            <label onClick={(e) => e.stopPropagation()}>
                <input className="me-1"
                       type="checkbox"
                       checked={listItem.isChecked}
                       onChange={handleCheckedChange}
                />
                {listItem.name}
                <span className="ms-1 text-muted">({listItem.quantity})</span>
            </label>
        </ListGroup.Item>
    );
};

const shoppingList = ({list}) => {
    const dispatch = useDispatch();

    const handleCardClick = () => {
        dispatch(selectList(list.id));
    }
    const handleCheckedChange = (itemId) => {
        const item = list.items.find(item => item.id === itemId);
        dispatch(updateShoppingListItem(list.id, {...item, isChecked: !item.isChecked}));
    };

    return (
        <Card className="h-100" onClick={handleCardClick}>
            <Card.Header>{list.name}</Card.Header>
            <Card.Body>
                <ListGroup variant="flush">
                    {list.items.length > 0 && list.items.map(item => (
                        <ShoppingListItem key={item.id} listItem={item} onCheckedChange={handleCheckedChange}/>
                    ))}
                </ListGroup>
                </Card.Body>
            </Card>
        );
};

export default shoppingList;