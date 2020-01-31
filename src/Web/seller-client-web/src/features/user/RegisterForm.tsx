import React, { useContext } from 'react'
import { Form as FinalForm, Field } from 'react-final-form'
import TextInput from '../../app/common/form/TextInput'
import { Form, Button, Label, Header } from 'semantic-ui-react'
import { UserFormValues } from '../../app/models/user'
import { FORM_ERROR } from 'final-form'
import { combineValidators, isRequired } from 'revalidate'
//import ErrorMessage from '../../app/common/form/ErrorMessage'

import { ApplicationState } from '../../app/stores';
import { useSelector, useDispatch } from 'react-redux';
import { actionCreators } from '../../app/reducers/UsersReducer';
import { RouteComponentProps } from 'react-router-dom'
import ErrorMessage from '../../app/common/form/ErrorMessage'

import { actionCreators as modalactions } from '../../app/reducers/ModalStore';

const validate = combineValidators({
    username: isRequired('username'),
    displayName: isRequired('displayName'),
    email: isRequired('email'),
    password: isRequired('password')
})
const RegisterForm: React.FC<RouteComponentProps> = ({ history }) => {
    const dispatch = useDispatch();
    async function register(values: UserFormValues) {
        await dispatch(actionCreators.register(values));
    }

    return (
        <FinalForm
            onSubmit={(values: UserFormValues) => register(values)
                .then(user => {
                    modalactions.closeModal();
                    history.push('/directsales');
                })
                .catch(error => ({
                    [FORM_ERROR]: error
                }))}
            validate={validate}
            render={({ handleSubmit, submitting, form, submitError, invalid, pristine, dirtyFieldsSinceLastSubmit }) => (
                <Form onSubmit={handleSubmit} error>
                    <Header as='h2' content='Signup' color='teal' />
                    <Field name='username' component={TextInput} placeholder='Username' />
                    <Field name='displayName' component={TextInput} placeholder='Display Name' />
                    <Field name='email' component={TextInput} placeholder='Email' />
                    <Field name='password' component={TextInput} placeholder='Password' type='password' />
                    {submitError && dirtyFieldsSinceLastSubmit && (<ErrorMessage error={submitError}  />)}
                    <Button disabled={invalid && !dirtyFieldsSinceLastSubmit || pristine} loading={submitting} color='teal' content='Register' fluid />

                </Form>

            )}
        />
    )
}
export default RegisterForm;