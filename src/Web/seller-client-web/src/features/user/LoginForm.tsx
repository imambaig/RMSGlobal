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

const LoginForm = () => {
    const selector = useSelector((state: ApplicationState) => state.users);
    const dispatch = useDispatch();
    async function login(values: UserFormValues) {
        console.log("b4 await dispatch");
        await dispatch(actionCreators.login(values));
        console.log("after await dispatch");
    }
    return (
        <FinalForm
            onSubmit={(values: UserFormValues) =>                 
                login(values).catch(error => 
                    
                    (
                        {

                            [FORM_ERROR]: error
                        }
                    )
                )
                //login(values).catch(error => console.log(error));
                //console.log("in submit");
            }
            render={({ handleSubmit,submitting,form,submitError }) => (
                <Form onSubmit={handleSubmit} >
                    <Header as='h2' content='Login to dotnetreact' color='teal' />
                    <Field name='email' component={TextInput} placeholder='Email' />
                    <Field name='password' component={TextInput} placeholder='Password' type='password' />
                    {submitError && <Label color='red' basic content={submitError.statusText} />}
                    <br />
                    <Button loading={submitting} positive content='Login' />
                    <pre> {JSON.stringify(form.getState(),null,2)} </pre>
                </Form>

            )}
        />
    )
}
export default LoginForm;