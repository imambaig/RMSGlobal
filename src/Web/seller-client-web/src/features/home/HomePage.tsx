import React, { useContext, Fragment } from 'react'
import { Container, Segment, Header, Button, Image, ModalContent } from 'semantic-ui-react'
import { Link, RouteComponentProps } from 'react-router-dom'
import { useSelector, useDispatch } from 'react-redux';
import { ApplicationState } from '../../app/stores';
import { actionCreators as modalactions} from '../../app/reducers/ModalStore';
import LoginForm from '../user/LoginForm';
import DirectSaleDetailedChat from '../directsales/details/DirectSaleDetailedChat';
import RegisterForm from '../user/RegisterForm';

const HomePage: React.FC<RouteComponentProps> = (routerprops) => {
    const userselector = useSelector((state: ApplicationState) => state.usersstate);
    const { isLoggedIn, user } = userselector!;

    const modalselector = useSelector((state: ApplicationState) => state.modalstate);
    const { } = modalselector!;
    const dispatch = useDispatch();

    return (
        <Segment inverted textAlign='center' vertical className='masthead' >
            <Container text>
                <Header as='h1' inverted>
                    <Image size='massive' src='/assets/logo.png' alt='logo' style={{ marginBottom: 12 }} />
                    Sales
            </Header>
                {isLoggedIn && user ? (
                    <Fragment>
                        <Header as='h2' inverted content={`Welcome back  ${user.displayName}`} />
                        <Button as={Link} to='/directsales' size='huge' inverted>
                            Sales
                 </Button>
                    </Fragment>
                ) : (
                        <Fragment>
                            <Header as='h2' inverted content='Welcome to Remarketing' />
                            <Button onClick={() => dispatch(modalactions.openModal(<LoginForm {...routerprops} /> ))}  size='huge' inverted>
                                Login
                            </Button>
                            <Button onClick={() => dispatch(modalactions.openModal(<RegisterForm {...routerprops} />))}  size='huge' inverted>
                                Register
                            </Button>
                        </Fragment>
                 )}
            
            </Container>
        </Segment>
        )
}
export default HomePage