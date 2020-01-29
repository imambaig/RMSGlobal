import React from 'react'
import { Segment, Grid, Icon } from 'semantic-ui-react'
import { DirectSale } from '../../../app/models/directsale'
import { format } from 'date-fns';

const DirectSaleDetailedInfo: React.FC<{ directsale: DirectSale }> = ({ directsale }) => {
    return (
        <Segment.Group>
            <Segment attached='top'>
                <Grid>
                    <Grid.Column width={1}>
                        <Icon size='large' color='teal' name='info' />
                    </Grid.Column>
                    <Grid.Column width={15}>
                        <p>{directsale.name}</p>
                    </Grid.Column>
                </Grid>
            </Segment>
            <Segment attached>
                <Grid verticalAlign='middle'>
                    <Grid.Column width={1}>
                        <Icon name='calendar' size='large' color='teal' />
                    </Grid.Column>
                    <Grid.Column width={15}>
                        <span>
                            {format(directsale.endDate, 'eeee do MMMM')} at 
                            {format(directsale.endDate, 'h:mm a')}
                        </span>
                    </Grid.Column>
                </Grid>
            </Segment>
            <Segment attached>
                <Grid verticalAlign='middle'>
                    <Grid.Column width={1}>
                        <Icon name='marker' size='large' color='teal' />
                    </Grid.Column>
                    <Grid.Column width={11}>
                        <span>{directsale.directSaleType}, {directsale.name}</span>
                    </Grid.Column>
                </Grid>
            </Segment>
        </Segment.Group>
    )
}

export default DirectSaleDetailedInfo