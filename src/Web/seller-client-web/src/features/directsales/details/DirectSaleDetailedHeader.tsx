import React from 'react'
import { Segment, Item, Button, Header, Image } from 'semantic-ui-react'
import { Link } from 'react-router-dom'
import { DirectSale } from '../../../app/models/directsale';

import { format } from 'date-fns';

const directSaleImageStyle = {
    filter: 'brightness(30%)'
};

const directSaleImageTextStyle = {
    position: 'absolute',
    bottom: '5%',
    left: '5%',
    width: '100%',
    height: 'auto',
    color: 'white'
};

const DirectSaleDetailedHeader: React.FC<{ directsale: DirectSale }> = ({ directsale }) => {
   
       return (
        <Segment.Group>
            <Segment basic attached='top' style={{ padding: '0' }}>
                   <Image src={`/assets/placeholder.png`} fluid style={directSaleImageStyle} />
                   <Segment basic style={directSaleImageTextStyle}>
                    <Item.Group>
                        <Item>
                            <Item.Content>
                                <Header
                                    size='huge'
                                       content={directsale.name}
                                    style={{ color: 'white' }}
                                />
                                   <p> {format(directsale.endDate,'eeee do MMMM')} </p>
                                <p>
                                    Hosted by <strong>Me</strong>
                                </p>
                            </Item.Content>
                        </Item>
                    </Item.Group>
                </Segment>
            </Segment>
            <Segment clearing attached='bottom'>
                   <Button color='teal'>  Join Direct Sale</Button>
                   <Button>Cancel Sale </Button>
                   <Button as={Link} to={`/manage/${directsale.id}`} color='orange'> Manage Sale </Button>
            </Segment>
        </Segment.Group>
    )
    
}

export default DirectSaleDetailedHeader