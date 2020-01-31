import React, { useContext } from 'react'
import { Form as FinalForm, Field } from 'react-final-form'
import TextInput from '../../app/common/form/TextInput'
import { Form, Button, Label, Header } from 'semantic-ui-react'
import { UserFormValues } from '../../app/models/user'
import { FORM_ERROR } from 'final-form'
import { combineValidators, isRequired } from 'revalidate'
import { ApplicationState } from '../../app/stores';
import { useSelector, useDispatch } from 'react-redux';
import { actionCreators } from '../../app/reducers/UsersReducer';
import { RouteComponentProps } from 'react-router-dom'
import ErrorMessage from '../../app/common/form/ErrorMessage'

import { actionCreators  as modalactions } from '../../app/reducers/ModalStore';
const validate = combineValidators({
    email: isRequired('email'),
    password: isRequired('password')
})
const LoginForm: React.FC<RouteComponentProps> = ({  history })  => {
    const selector = useSelector((state: ApplicationState) => state.usersstate);
    const dispatch = useDispatch();
    async function login(values: UserFormValues) {
        console.log("b4 await dispatch");
        await dispatch(actionCreators.login(values));
        console.log("after await dispatch");
    }
    return (
        <FinalForm
            onSubmit={(values: UserFormValues) =>
                login(values).then(user =>
                {
                    modalactions.closeModal();
                    history.push('/directsales');
                })
                    .catch(error =>
                    (
                        {

                            [FORM_ERROR]: error
                        }
                    )
                )
                //login(values).catch(error => console.log(error));
                //console.log("in submit");
            }
            validate={validate}
            render={({ handleSubmit,submitting,form,submitError, invalid, pristine , dirtySinceLastSubmit}) => (
                <Form onSubmit={handleSubmit} error>

                    <Header as='h2' content='Login to dotnetreact' color='teal' textAlign='center'/>
                    <Field name='email' component={TextInput} placeholder='Email' />
                    <Field name='password' component={TextInput} placeholder='Password' type='password' />
                    {submitError && !dirtySinceLastSubmit && <ErrorMessage error={submitError} text ='Invalid email or password' />}
                    <br />
                    <Button disabled={invalid && !dirtySinceLastSubmit ||pristine} loading={submitting} positive content='Login' />
                   
                </Form>

            )}
        />
    )
}
export default LoginForm;