import React, { useEffect } from 'react'
import { Grid } from 'semantic-ui-react'
import DirectSaleList from './DirectSaleList'
import DirectSaleDetails from '../details/DirectSaleDetails'
import DirectSaleForm from '../form/DirectSaleForm'


import { ApplicationState } from '../../../app/stores';
import { useSelector, useDispatch } from 'react-redux'
import LoadingComponent from '../../../app/layout/LoadingComponent'
import { actionCreators } from '../../../app/reducers/DirectSalesReducer'


const DirectSaleDashboard: React.FC = () => {
    const selector = useSelector((state: ApplicationState) => state.directsales);
    const dispatch = useDispatch();
    const ensureDataFetched = () => {
        console.log('ensureDataFetched');
        dispatch(actionCreators.requestDirectSales());
    }


    useEffect(() => {
        ensureDataFetched();
    }, []);
    if (selector!.isLoading) return <LoadingComponent content='loading ....' />
    return (
        <Grid>
            <Grid.Column width={10}>
                <DirectSaleList  />
                
            </Grid.Column>
            <Grid.Column width={6}>
               <h2> Filters </h2>
                
            </Grid.Column>
        </Grid>
    )
}
export default DirectSaleDashboard