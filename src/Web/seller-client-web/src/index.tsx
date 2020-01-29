import React from 'react';
import ReactDOM from 'react-dom';
import { createBrowserHistory } from 'history';
import 'react-toastify/dist/ReactToastify.min.css';
import 'react-widgets/dist/css/react-widgets.css';
import './app/layout/styles.css';
import App from './app/layout/App';
import * as serviceWorker from './serviceWorker';
import configureStore from "./app/stores/configureStore";
import { Provider } from "react-redux";
import { Router } from 'react-router-dom';
import ScrollToTop from './app/layout/ScrollToTop';
import dateFnsLocalizer from 'react-widgets-date-fns';
//import ScrollToTop from './app/layout/ScrollToTop';

dateFnsLocalizer();

// Create browser history to use in the Redux store
//const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
export const history = createBrowserHistory();
// Get the application-wide store instance, prepopulating with state from the server where available.
const store = configureStore(history);
//const store = createStore(reducers, applyMiddleware(thunk));
//ReactDOM.render(<App />, document.getElementById('root'));
ReactDOM.render(
    <Provider store={store}>
        <Router history={history}>    
            <ScrollToTop>
                <App />    
            </ScrollToTop>
        </Router>
    </Provider>
    , document.getElementById("root"));

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
