import {useDispatch, useSelector} from "react-redux";
import {useEffect} from "react";
import 'bootstrap-icons/font/bootstrap-icons.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";

import {initialiseShoppingLists} from "./reducers/shoppingListReducer.js";
import ShoppingList from "./components/ShoppingList.jsx";
import ShoppingListModal from "./components/ShoppingListModal.jsx";
import {selectList} from "./reducers/uiReducer.js";
import NewShoppingListForm from "./components/NewShoppingListForm.jsx";

const App = () => {
    const dispatch = useDispatch();

    const shoppingLists = useSelector(state => state.shoppingLists);
    const selectedListId = useSelector(state => state.ui.selectedListId);
    const showModal = selectedListId !== null;

    useEffect(() => {
            dispatch(initialiseShoppingLists());
        }
        , [dispatch]
    );

    const handleModalClose = () => {
        dispatch(selectList(null));
    }

    return (
        <Container className="py-3">
            <Row className="justify-content-md-center mb-3">
                <Col xs={12} md={8}>
                    <h1 className="display-6 text-center">
                        <span className="text-success">
                            <i className="bi bi-cart"></i>
                        </span>
                        Your Shopping Lists
                    </h1>
                </Col>
            </Row>
            <Row className="justify-content-md-center mb-3">
                <NewShoppingListForm />
            </Row>
            <Row xs={1} md={3} className="g-3">
                {shoppingLists.map(list => (
                    <Col key={list.id}>
                        <ShoppingList list={list}/>
                    </Col>
                ))}
            </Row>
            <ShoppingListModal onClose={handleModalClose} show={showModal}/>
        </Container>
    )
};

export default App;