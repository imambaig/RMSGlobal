import React, {  useEffect, Fragment } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {   Container } from 'semantic-ui-react'
import NavBar from '../../features/nav/NavBar';
import DirectSaleDashboard from '../../features/directsales/dashboard/DirectSaleDashboard';
import { ApplicationState, } from '../stores/';
import { Route, withRouter, RouteComponentProps, Switch } from 'react-router-dom';
import HomePage from '../../features/home/HomePage';
import DirectSaleForm from '../../features/directsales/form/DirectSaleForm';
import DirectSaleDetails from '../../features/directsales/details/DirectSaleDetails';
import NotFound from "../layout/NotFound";
import {ToastContainer} from 'react-toastify'
import LoginForm from '../../features/user/LoginForm';

const App: React.FC<RouteComponentProps> = ({location }) => {  

    const selector = useSelector((state: ApplicationState) => state.directsales);
    const dispatch = useDispatch();


    return (
        <Fragment>
            <ToastContainer position='bottom-right' />
            <Route exact path='/' component={HomePage} />
            <Route path={'/(.+)'} render={() => (
                <Fragment>
                    <NavBar />
                    <Container style={{ marginTop: '7em' }}>
                        <Switch>
                            <Route exact path='/directsales' component={DirectSaleDashboard} />
                            <Route path='/directsales/:id' component={DirectSaleDetails} />
                            <Route key={location.key} path={['/createdirectsale', '/manage/:id']} component={DirectSaleForm} />
                            <Route path="/Login" component={LoginForm} />
                            <Route component={NotFound} />
                        </Switch>
                        
                    </Container>
                </Fragment>

            )} />
           
        </Fragment>       
    );
}

export default withRouter(App);
