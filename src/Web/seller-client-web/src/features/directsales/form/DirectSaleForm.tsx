
import React, { useState, FormEvent, useEffect } from 'react'
import { Segment, Form, Button, Grid, Message } from 'semantic-ui-react'
import { DirectSale, DiretSaleFormValues, DiretSaleFormValue } from '../../../app/models/directsale'
import { v4 as uuid } from 'uuid';
import { useDispatch, useSelector } from 'react-redux'
import { actionCreators } from '../../../app/reducers/DirectSalesReducer';
import { ApplicationState } from '../../../app/stores';
import { RouteComponentProps } from 'react-router-dom';
import { Form as FinalForm,Field } from 'react-final-form';
import TextInput from '../../../app/common/form/TextInput';
import TextAreaInput from '../../../app/common/form/TextAreaInput';
import SelectInput from '../../../app/common/form/SelectInput';
import { directsaleOptions } from '../../../app/common/options/directsaleOptions';
import DateInput from '../../../app/common/form/DateInput';
import { combineDateAndTime } from '../../../app/common/util/util';

import { combineValidators, isRequired, composeValidators, hasLengthGreaterThan } from 'revalidate';

const validate = combineValidators({
    name: composeValidators(
        isRequired('Name'),
        hasLengthGreaterThan(4)({ message: 'Description needsd to be atleast 5 characters' }))(),
    directSaleType: isRequired('DirectSaleType'),
    endDate: isRequired('EndDate'),
    time: isRequired('EndTime')

})

interface DetailParams {
    id: string
}

const DirectSaleForm: React.FC<RouteComponentProps<DetailParams>> = ({ match,history }) => {
    const selector = useSelector((state: ApplicationState) => state.directsalesstate);
    const { directsale: initialFormState,submitting } = selector!;
    const dispatch = useDispatch();
    async function fetchData() {
            if (match.params.id) {
                await dispatch(actionCreators.loadDirectSale(match.params.id));
                console.log(initialFormState);
                initialFormState && setDirectSale(initialFormState);
            }
        };
   

  

    const [directsale, setDirectSale] = useState(new DiretSaleFormValue());
    const [loading, setLoading] = useState(false);
    useEffect(() => {
        // should resolve page refresh issue   ********************8 initialise form with data
        // fetchData();  
        
        if (match.params.id && !directsale.id) {
            //setLoading(true); // may need to revist
            dispatch(actionCreators.loadDirectSale(match.params.id));
            initialFormState && setDirectSale(new DiretSaleFormValue(initialFormState));
           // setLoading(false); // may need to revist
        }
        return () => {
            dispatch(actionCreators.clearDirectSale());
        }
    }, [actionCreators.loadDirectSale, actionCreators.clearDirectSale, match.params.id, initialFormState, directsale.id])
    //const handleSubmit = async () => {
    //    console.log(directsale);
    //    if (directsale.id.length === 0) {
    //        let newDirectSale = {
    //            ...directsale,
    //            id: uuid()
    //        }
    //        await dispatch(actionCreators.createDirectSale(newDirectSale));
    //        history.push(`/directsales/${newDirectSale.id}`);

    //    } else {
    //        await dispatch(actionCreators.editDirectSale(directsale));
    //        history.push(`/directsales/${directsale.id}`)
    //    }
    //}

    const handleFinalFormSubmit = async (values: any) => {
        const dateAndTime = combineDateAndTime(values.endDate, values.time);
        const { date, time, ...directsale } = values; // exclude date time from directsale
        directsale.date = dateAndTime;
        if (!directsale.id) {
            let newDirectSale = {
                ...directsale,
                id: uuid()
            }
            await dispatch(actionCreators.createDirectSale(newDirectSale));
            history.push(`/directsales/${newDirectSale.id}`);

        } else {
            await dispatch(actionCreators.editDirectSale(directsale));
            history.push(`/directsales/${directsale.id}`)
        }
    }

   
    return (
        <Grid>
            <Grid.Column>
                <Segment clearing>
                    <FinalForm
                        validate={validate}
                        initialValues={directsale}
                        onSubmit={handleFinalFormSubmit}
                        render={({ handleSubmit,invalid,pristine }) => (
                            <Form loading={loading} onSubmit={handleSubmit}>
                                <Field name='name' component={TextAreaInput} rows={2} placeholder='Name' value={directsale.name} />
                                <Form.Group widths='equal'>
                                    <Field name='endDate' date={true} component={DateInput} type='datetime-local' placeholder='endDate' value={directsale.endDate} />
                                    <Field name='time' time={true} component={DateInput} type='datetime-local' placeholder='Time' value={directsale.time} />
                                </Form.Group>
                                <Field name='directSaleType' component={SelectInput} options={directsaleOptions} placeholder='Direct Sale Type' value={directsale.directSaleType}  />
                                <Button loading={submitting} disabled={loading || invalid || pristine} floated='right' positive type='submit' content='Submit' />
                                <Button disabled={loading} onClick={directsale.id ? () => history.push(`/directsales/${directsale.id}`)
                                                                                  : () => history.push('/directsales')} floated='right' type='button' content='Cancel' />
                            </Form>
                            )}
                    />
                 
                </Segment>
            </Grid.Column>
        </Grid>
       
    )
}
export default DirectSaleForm