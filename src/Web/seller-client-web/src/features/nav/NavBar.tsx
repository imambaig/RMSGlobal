import React from 'react'
import { Menu, Container, Button, Dropdown, Image } from 'semantic-ui-react'
import {  useDispatch, useSelector } from 'react-redux'
import { actionCreators } from '../../app/reducers/DirectSalesReducer';
import { actionCreators as useractions } from '../../app/reducers/UsersReducer';
import { Link, NavLink, RouteComponentProps } from 'react-router-dom';
import { ApplicationState } from '../../app/stores';


const NavBar: React.FC = () => {
    const dispatch = useDispatch();
    const selector = useSelector((state: ApplicationState) => state.usersstate);
    const { isLoggedIn, user } = selector!;
    const handleLogout = ()=>{
        dispatch(useractions.logout());
        // shoud check how to use history object to send back to login page
    }
    return (
        <Menu fixed='top' inverted>
            <Container>
                <Menu.Item header as={NavLink} exact to='/'>
                    <img src="/assets/logo.png" alt="logo" style={{ marginRight: '10px' }} />
                    Direct Sales
                </Menu.Item>
                <Menu.Item
                    name='DirectSales' as={NavLink} to='/directsales'
                />
                <Menu.Item>
                    <Button as={NavLink} to='/createdirectsale' positive content="Create DS" />
                </Menu.Item>
                {user && (
                    <Menu.Item position='right'>
                        <Image avatar spaced='right' src={user.image ??'/assets/user.png'} />
                        <Dropdown pointing='top left' text={user.displayName}>
                            <Dropdown.Menu>
                                <Dropdown.Item as={Link} to={`/profile/username`} text='My profile' icon='user' />
                                <Dropdown.Item onClick={() => handleLogout()} to='/'  text='Logout' icon='power'  />
                            </Dropdown.Menu>
                        </Dropdown>
                    </Menu.Item>
                    )
                }
            </Container>

        </Menu>
    )
}

export default NavBar
