import { applyMiddleware, combineReducers, compose, createStore } from "redux";
import thunk from "redux-thunk";
import { connectRouter, routerMiddleware } from "connected-react-router";
import { History } from "history";
import { ApplicationState, reducers } from ".";

export default function configureStore(
    history: History,
    initialState?: ApplicationState
) {
    const middleware = [thunk, routerMiddleware(history)];

    function saveToLocalStorage(state:any) {
        try {
            const serializedState = JSON.stringify(state)            
            localStorage.setItem('state',serializedState)
        } catch (e) {
            console.log(e);
        }
    }

    function loadFromLocalStorage() {
        try {
            const serializedState = localStorage.getItem('state')
            if (serializedState === null) return undefined
            return JSON.parse(serializedState)
        } catch (e) {
            console.log(e);
            return undefined;
        }
    }
    const persistedState = loadFromLocalStorage()
    const rootReducer = combineReducers({
        ...reducers,
        router: connectRouter(history)
    });

    const enhancers = [];
    const windowIfDefined =
        typeof window === "undefined" ? null : (window as any);
    if (windowIfDefined && windowIfDefined.__REDUX_DEVTOOLS_EXTENSION__) {
        enhancers.push(windowIfDefined.__REDUX_DEVTOOLS_EXTENSION__());
    }

    const store= createStore(
        rootReducer,
        initialState,
       //persistedState,
        compose(applyMiddleware(...middleware), ...enhancers)
    );
    store.subscribe(()=>saveToLocalStorage(store.getState()))
    return store;
}
