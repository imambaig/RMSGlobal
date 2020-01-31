import React, { useContext } from 'react'
import { Modal } from 'semantic-ui-react'
import { ApplicationState } from '../../stores';
import { useSelector, useDispatch } from 'react-redux';
import { actionCreators } from '../../../app/reducers/ModalStore';

const ModalContainer = () => {
    //const { modal: { open, body }, closeModal } = rootStore.modalStore;
    const selector = useSelector((state: ApplicationState) => state.modalstate);
    const { body, open } = selector!;
    const dispath = useDispatch();
    return (
        <Modal open={open} onClose={()=>dispath(actionCreators.closeModal())} size='mini'>
            <Modal.Content>{body}</Modal.Content>
        </Modal>
    )
}

export default ModalContainer