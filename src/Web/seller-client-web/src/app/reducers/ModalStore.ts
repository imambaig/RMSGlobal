import { Action, Reducer } from "redux";
import { AppThunkAction } from "../stores";
import { User, UserFormValues } from "../models/user";
import agent from "../api/agent";
import { getTokenFromLocalstore, saveTokenToLocalstore } from "../common/util/util";
// STATE - This defines the type of data maintained in the Redux store.

export interface ModalState {
    open: boolean;
    body:any;
}

// ACTIONS
type OpenModalAction = {
    type: "OPEN_MODAL",
    content: any
}
type CloseModalAction = {
    type: "CLOSE_MODAL"
}

// Declare a 'discriminated union' type.

type KnownAction = OpenModalAction | CloseModalAction;

// ACTION CREATORS
export const actionCreators = {
    openModal: (content: any): AppThunkAction<KnownAction> => async (
        dispatch,
        getState
    ) =>
        new Promise(function (resolve, reject) {
            const appState = getState();
            if (appState && appState.usersstate) {
                dispatch({
                    type: "OPEN_MODAL",
                    content: content
                });


            }
        }
        ),//promise
    closeModal: (): AppThunkAction<KnownAction> => async (
        dispatch,
        getState
    ) =>
        new Promise(function (resolve, reject) {
            const appState = getState();
            if (appState && appState.usersstate) {
                dispatch({
                    type: "CLOSE_MODAL"
                });
            }
        }
       )
}

// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: ModalState = {
    open: false,
    body:null
};

export const reducer: Reducer<ModalState> = (
    state: ModalState | undefined,
    incomingAction: Action
): ModalState => {
    if (state === undefined) {
        return unloadedState;
    }
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "OPEN_MODAL":
            return {
                ...state,
                open: true,
                body:action.content
            };
        case "CLOSE_MODAL":
            //console.log(action.user);
            return {
                ...state,
                open: false,
                body: null
            };
            break;

    }
    return state || unloadedState;
}
