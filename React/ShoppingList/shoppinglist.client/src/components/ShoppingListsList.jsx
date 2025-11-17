import {useDispatch, useSelector} from "react-redux";

const ShoppingListItem = ({listItem}) => 
    <div>
        {listItem.name}
    </div>;

const shoppingListsList = () => {
    const dispatch = useDispatch();
    
    const shoppingLists = useSelector(state => state.shoppingLists);
    
    return <>
        {shoppingLists.map(list => <ShoppingListItem key={list.id} listItem={list} />)}
    </>
}

export default shoppingListsList;