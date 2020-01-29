import React from 'react'
import { Item, Button, Label, Segment, Icon } from 'semantic-ui-react'
import { Link } from 'react-router-dom'
import { DirectSale } from '../../../app/models/directsale';
import { format } from 'date-fns';

const DirectSaleListItem: React.FC<{ directsale: DirectSale }> = ({ directsale}) => {
    return (
        <Segment.Group>
            <Segment>
                <Item.Group>
                    <Item>
                        <Item.Image size='tiny' circular src='/assets/user.png' />
                        <Item.Content>
                            <Item.Header as='a'>{directsale.directSaleType}</Item.Header>
                            <Item.Description> {directsale.name}  </Item.Description>
                        </Item.Content>
                    </Item>
                </Item.Group>                
            </Segment>
            <Segment>
                <Icon name='clock' /> {format(directsale.endDate,'h:mm a')}
            </Segment>
            <Segment secondary>

            </Segment>
            <Segment clearing>
                <Button as={Link} to={`/directsales/${directsale.id}`} floated='right' content='View' color='blue' />
            </Segment>
        </Segment.Group>
        
        )
}

export default DirectSaleListItem




//import React from 'react'

//const DirectSaleListItem = () => {
//    return (
//        <div>
//        </div>
//    )
//}

//export default DirectSaleListItem