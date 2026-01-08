import Modal from "react-bootstrap/Modal";
import {useSelector} from "react-redux";
import ShoppingListForm from "./ShoppingListForm.jsx";

const ShoppingListModal = ({onClose, show}) => {
    const selectedList = useSelector(state => state.shoppingLists.find(list => list.id === state.ui.selectedListId));
    
    return (
        <Modal show={show} onHide={onClose} centered={true}>
            <ShoppingListForm list={selectedList}/>
        </Modal>
    );
};

export default ShoppingListModal;