import React, { useState, useEffect, Fragment } from 'react'
import { Item, Button, Label, Segment } from 'semantic-ui-react'
import { ApplicationState } from '../../../app/stores';
import { useSelector, useDispatch } from 'react-redux'
import DirectSaleListItem from './DirectSaleListItem';
import { DirectSale } from '../../../app/models/directsale';
import { format} from 'date-fns'

const DirectSaleList = () => {
    const selector = useSelector((state: ApplicationState) => state.directsalesstate);
    const { directsales } = selector!;
    const [directsalesByDate, setDirectSalesByDate] = useState< [string, DirectSale[] ][]>([])
    //const [directsalesByDate, setDirectSalesByDate] = useState<DirectSale[]>([]);
    const groupDirectSalesByDate = (directsales: DirectSale[]): [string, DirectSale[]][]=>{
        console.log(directsales);
        const sortedDirectSales = directsales.sort((a, b) => new Date(a.endDate).getTime() - new Date(b.endDate).getTime());
        //const sortedDirectSales = directsales;//.sort((a, b) => a.id - b.id);
        return Object.entries(sortedDirectSales.reduce((directsales, directsale) => {
            const date = directsale.endDate.toISOString().split('T')[0];
            directsales[date] = directsales[date] ? [...directsales[date], directsale] : [directsale];
            
            return directsales;
            //setDirectSalesByDate(directsales);
        }, {} as { [key: string]: DirectSale[] })); 
        
    }


    useEffect(() => {
        setDirectSalesByDate(groupDirectSalesByDate(directsales)); // check this issue
        //setDirectSalesByDate(directsales);
       //setDirectSalesByDate(directsales.sort((a, b) => Date.parse(a.endDate) - Date.parse(b.endDate)));
       //console.log(setDirectSalesByDate);
        //{format(group,'eeee do MMMM')}
    }, [directsalesByDate && directsalesByDate.length]);
    return (
        <Fragment>
            {directsalesByDate && directsalesByDate.map(([group, directsales]) => (
                <Fragment key={group}>
                    <Label  size='large' color='blue'>
                        {group}
                    </Label>
                    <Segment clearing>
                        <Item.Group divided>
                            {directsales.map((directsale) => (

                                <DirectSaleListItem key={directsale.id} directsale={directsale} />
                            ))}
                        </Item.Group>

                    </Segment>
                </Fragment>

                
            ))}
        </Fragment>

        //<Fragment>
        //            <Segment clearing>
        //                <Item.Group divided>
        //            {directsalesByDate && directsalesByDate.map((directsale) => (

        //                        <DirectSaleListItem key={directsale.id} directsale={directsale} />
        //                    ))}
        //                </Item.Group>

        //            </Segment>
        //</Fragment>
       
    )
}
export default DirectSaleList