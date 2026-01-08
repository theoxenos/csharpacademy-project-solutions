import {useDispatch, useSelector} from "react-redux";
import {useCallback, useEffect} from "react";
import 'bootstrap-icons/font/bootstrap-icons.css';
import 'bootstrap/dist/css/bootstrap-reboot.min.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";

import {initialiseShoppingLists} from "./reducers/shoppingListReducer.ts";
import ShoppingList from "./components/ShoppingList.tsx";
import ShoppingListModal from "./components/ShoppingListModal.tsx";
import {selectList} from "./reducers/uiReducer.ts";
import NewShoppingListForm from "./components/NewShoppingListForm.tsx";
import LoginForm from "./components/LoginForm.tsx";
import type {ShoppingList as IShoppingList, UiState, User} from "./types.ts";
import type {AppDispatch, RootState} from "./store.ts";

const App = () => {
    const dispatch: AppDispatch = useDispatch();

    const shoppingLists: IShoppingList[] = useSelector((state: RootState) => state.shoppingLists);
    const selectedListId = useSelector<{ ui: UiState }>(state => state.ui.selectedListId);
    const showModal = selectedListId !== null;

    const user = useSelector<{ user: User }>(state => state.user);

    useEffect(() => {
        void dispatch(initialiseShoppingLists());
    }, [dispatch]);

    const handleModalClose = useCallback(() => {
        dispatch(selectList(null));
    }, [dispatch]);

    if (!user) {
        return <LoginForm/>;
    }

    return (
        <Container className="py-3">
            <Row className="justify-content-md-center mb-3">
                <Col xs={12} md={8}>
                    <h1 className="display-6 text-center">
                        <span className="text-success">
                            <i className="bi bi-cart"/>
                        </span>
                        {"Your Shopping Lists"}
                    </h1>
                </Col>
            </Row>
            <Row className="justify-content-md-center mb-3">
                <NewShoppingListForm/>
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
    );
};

export default App;