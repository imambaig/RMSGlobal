import React, { useEffect } from 'react'
import {  Button, Grid } from 'semantic-ui-react'
import { useSelector, useDispatch } from 'react-redux'
import { ApplicationState } from '../../../app/stores'
import { actionCreators } from '../../../app/reducers/DirectSalesReducer';
import { RouteComponentProps } from 'react-router-dom';
import LoadingComponent from '../../../app/layout/LoadingComponent';
import DirectSaleDetailedHeader from './DirectSaleDetailedHeader';
import DirectSaleDetailedInfo from './DirectSaleDetailedInfo';
import DirectSaleDetailedChat from './DirectSaleDetailedChat';
import DirectSaleDetailedSidebar from './DirectSaleDetailedSidebar';

interface DetailParams {
    id:string
}

const DirectSaleDetails: React.FC<RouteComponentProps<DetailParams>> = ({match,history}) => {
    const selector = useSelector((state: ApplicationState) => state.directsalesstate);
    const { directsale } = selector!;
    const dispatch = useDispatch();
    useEffect(() => {
        dispatch(actionCreators.loadDirectSale(match.params.id));
    }, [])
    if (selector!.isLoading || !directsale) return <LoadingComponent content="loading Direct Sales" />

    if (!directsale)
            return<h2> Direct Sale Not FOund </h2>
    return (
        <Grid>
            <Grid.Column width={10}>
                <DirectSaleDetailedHeader directsale={directsale}  />
               <DirectSaleDetailedInfo directsale={directsale} />
                <DirectSaleDetailedChat /> 
            </Grid.Column>
            <Grid.Column width={6}>
                <DirectSaleDetailedSidebar />
            </Grid.Column>
        </Grid>
    )
}
export default DirectSaleDetails