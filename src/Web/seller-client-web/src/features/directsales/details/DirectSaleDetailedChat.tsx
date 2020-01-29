import React, { Fragment } from 'react'
import { Segment, Header, Form, Button, Comment } from 'semantic-ui-react'
const DirectSaleDetailedChat = () => {
    return (
        <Fragment>
            <Segment
                textAlign='center'
                attached='top'
                inverted
                color='teal'
                style={{ border: 'none' }}
            >
                <Header>Chat about this event</Header>
            </Segment>
            <Segment attached>
                <Comment.Group>
                   
                        <Comment >
                        <Comment.Avatar src=' /assets/user.png' />
                            <Comment.Content>
                                <Comment.Author as='a' >John</Comment.Author>
                                <Comment.Metadata>
                                    <div>Today at 5:42PM</div>
                                </Comment.Metadata>
                                <Comment.Text>Going to be fun!</Comment.Text>
                            </Comment.Content>
                        </Comment>
                    <Comment >
                        <Comment.Avatar src=' /assets/user.png' />
                        <Comment.Content>
                            <Comment.Author as='a' >Peter</Comment.Author>
                            <Comment.Metadata>
                                <div>Today at 5:56PM</div>
                            </Comment.Metadata>
                            <Comment.Text>Exciting!</Comment.Text>
                        </Comment.Content>
                    </Comment>

                    <Form reply>
                        <Form.TextArea />
                        <Button
                            content='Add Reply'
                            labelPosition='left'
                            icon='edit'
                            primary
                        />
                    </Form>

                </Comment.Group>
            </Segment>
        </Fragment>
    )
}

export default DirectSaleDetailedChat