import Modal from "react-bootstrap/Modal";
import {useSelector} from "react-redux";
import ShoppingList from "./ShoppingList.jsx";

const ShoppingListModal = ({onClose, show}) => {
    const selectedList = useSelector(state => state.shoppingLists.find(list => list.id === state.ui.selectedListId));
    
    return (
        <Modal show={show} onHide={onClose} centered={true}>
            {selectedList && <ShoppingList list={selectedList}/>}
        </Modal>
    )
};

export default ShoppingListModal;