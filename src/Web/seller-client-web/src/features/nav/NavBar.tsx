import React from 'react'
import { Menu, Container, Button } from 'semantic-ui-react'
import {  useDispatch } from 'react-redux'
import { actionCreators } from '../../app/reducers/DirectSalesReducer';
import { Link, NavLink } from 'react-router-dom';


const NavBar= () => {
    const dispatch = useDispatch();
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
            </Container>

        </Menu>
    )
}

export default NavBar
