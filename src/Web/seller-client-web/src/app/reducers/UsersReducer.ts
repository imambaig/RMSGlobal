import { Action, Reducer } from "redux";
import { AppThunkAction } from "../stores";
import { User, UserFormValues } from "../models/user";
import agent from "../api/agent";
import { getTokenFromLocalstore, saveTokenToLocalstore } from "../common/util/util";
// STATE - This defines the type of data maintained in the Redux store.

export interface UserState {
    isLoading: boolean;
    users: User[];
    user: User | null;
    submitting: boolean;
    error: any;
    isLoggedIn: boolean;

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

type LogoutAction = {
    type:"LOGOUT"
}

type RequestUserAction = {
    type: "REQUEST_USER"
}
type ReceiveUserAction = {
    type: "RECEIVE_USER",
    user: User
}

type RequestRegisterAction = {
    type: "REQUEST_REGISTER",
    values: UserFormValues
}
type ReceiveRegisterAction = {
    type: "RECEIVE_REGISTER",
    user: User
}


// Declare a 'discriminated union' type.

type KnownAction = RequestLoginAction | ReceiveLoginAction | FailedLoginAction | LogoutAction
    | RequestUserAction | ReceiveUserAction |RequestRegisterAction |ReceiveRegisterAction;

// ACTION CREATORS -https://medium.com/collaborne-engineering/returning-promises-from-redux-action-creators-3035f34fa74b
export const actionCreators = {
    login: (values: UserFormValues): AppThunkAction<KnownAction> => async (
        dispatch,
        getState
    ) =>
    new Promise(function (resolve, reject)
    {
        const appState = getState();
        if (appState && appState.usersstate ) {
            dispatch({
                type: "REQUEST_LOGIN",
                values: values
            });
            if (appState.usersstate.user && appState.usersstate.user.token && appState.usersstate.user.username === values.username) {
                dispatch({
                    type: "RECEIVE_LOGIN",
                    user: appState.usersstate.user
                });
            } else {
                agent.Users.login(values)
                    .then(response => response as User)
                    .then(data => {
                        saveTokenToLocalstore(data.token);
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
        ),//promise
    logout: () => (
        { type: "LOGOUT" } as LogoutAction),
    user: (value: string): AppThunkAction<KnownAction> => async (
        dispatch,
        getState
    ) =>
        new Promise(function (resolve, reject) {
            const appState = getState();
            if (appState && appState.usersstate) {
                dispatch({
                    type: "REQUEST_USER"
                });
                if (appState.usersstate.user && appState.usersstate.user.token ) {
                    dispatch({
                        type: "RECEIVE_USER",
                        user: appState.usersstate.user
                    });
                } else {
                    agent.Users.current()
                        .then(response => response as User)
                        .then(data => {
                            saveTokenToLocalstore(data.token);
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
        ),//promise
    register: (values: UserFormValues): AppThunkAction<KnownAction> => async (
        dispatch,
        getState
    ) =>
        new Promise(function (resolve, reject) {
            const appState = getState();
            if (appState && appState.usersstate) {
                dispatch({
                    type: "REQUEST_REGISTER",
                    values: values
                });
                agent.Users.register(values)
                    .then(response => response as User)
                    .then(data => {
                        saveTokenToLocalstore(data.token);
                        dispatch({
                            type: "RECEIVE_REGISTER",
                            user: data
                        });
                        resolve();
                    }).catch(error => {
                        reject(error);
                    });

            }
        }
        ),//promise
}

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: UserState = {
    users: [],
    isLoading: false,
    user: null,
    submitting: false,
    error: '',
    isLoggedIn:false
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
                ...state,
                isLoading: true
            };
        case "RECEIVE_LOGIN":
           //console.log(action.user);
           return {
               ...state,
                user: action.user,
               isLoading: false,
               isLoggedIn:true
            };
            break;
        case "FAILED_LOGIN":
            console.log(action.error);
            return {
                ...state,
                error:action.error,
                isLoading: false
            };
        case "LOGOUT":
            saveTokenToLocalstore(null);
            return {
                ...state,
                user:null
            }
        case "REQUEST_USER":
            return {
                ...state,
                isLoading: true
            };
        case "RECEIVE_USER":
            //console.log(action.user);
            return {
                ...state,
                user: action.user,
                isLoading: false,
                isLoggedIn: true
            };
            break;
        case "REQUEST_REGISTER":
            return {
                ...state,
                isLoading: true
            };
        case "RECEIVE_REGISTER":
            //console.log(action.user);
            return {
                ...state,
                user: action.user,
                isLoading: false,
                isLoggedIn: true
            };
            break;
    }
    return state || unloadedState;
}
