import { Action, Reducer } from "redux";
import { AppThunkAction } from "../stores";
import { User, UserFormValues } from "../models/user";
import agent from "../api/agent";
// STATE - This defines the type of data maintained in the Redux store.

export interface UserState {
    isLoading: boolean;
    users: User[];
    user: User | null;
    submitting: boolean;
    error: any;

}

// ACTIONS

type RequestLoginAction={
    type: "REQUEST_LOGIN",
    values: UserFormValues
}
type ReceiveLoginAction = {
    type: "RECEIVE_LOGIN",
    user:User
}
type FailedLoginAction = {
    type: "FAILED_LOGIN",
    error: string
}

// Declare a 'discriminated union' type.

type KnownAction = RequestLoginAction | ReceiveLoginAction | FailedLoginAction;

// ACTION CREATORS -https://medium.com/collaborne-engineering/returning-promises-from-redux-action-creators-3035f34fa74b
export const actionCreators = {
    login: (values: UserFormValues): AppThunkAction<KnownAction> => async (
        dispatch,
        getState
    ) =>
    new Promise(function (resolve, reject)
    {
        const appState = getState();
        if (appState && appState.users ) {
            dispatch({
                type: "REQUEST_LOGIN",
                values: values
            });
            if (appState.users.user && appState.users.user.token && appState.users.user.username === values.username) {
                dispatch({
                    type: "RECEIVE_LOGIN",
                    user: appState.users.user
                });
            } else {
                agent.Users.login(values)
                    .then(response => response as User)
                    .then(data => {
                        dispatch({
                            type: "RECEIVE_LOGIN",
                            user: data
                        });
                        resolve();
                    }).catch(error => {
                        console.log("reducer catch");
                        console.log(error);
                        reject(error);
                        //dispatch({
                        //    type: "FAILED_LOGIN",
                        //    error:error
                        //})
                    });
            }
            
        }
        }
    )//promise
}

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: UserState = {
    users: [],
    isLoading: false,
    user: null,
    submitting: false,
    error:''
};
export const reducer: Reducer<UserState> = (
    state: UserState | undefined,
    incomingAction: Action
): UserState => {
    if (state === undefined) {
        return unloadedState;
    }
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_LOGIN":
            return {
                ...unloadedState,
                isLoading: true
            };
        case "RECEIVE_LOGIN":
           //console.log(action.user);
           return {
                ...unloadedState,
                user: action.user,
                isLoading: false
            };
            break;
        case "FAILED_LOGIN":
            console.log(action.error);
            return {
                ...unloadedState,
                error:action.error,
                isLoading: false
            };
    }
    return unloadedState;
}
