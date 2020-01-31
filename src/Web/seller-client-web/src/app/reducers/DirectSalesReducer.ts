import { Action, Reducer } from "redux";
import { AppThunkAction } from "../stores";
import { DirectSale } from "../models/directsale";
import agent from "../api/agent";
import { history } from '../..';
// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface DirectSaleState {
    isLoading: boolean;
    directsales: DirectSale[];
    directsale: DirectSale | null;
    editMode: boolean;
    submitting: boolean;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestDirectSalesAction {
    type: "REQUEST_DIRECTSALES_LIST";
}

interface ReceiveDirectSalesAction {
    type: "RECEIVE_DIRECTSALES_LIST";
    directsales: DirectSale[];
}

export interface SelectDirectSaleAction { type: 'SELECT_DIRECTSALE'; id: string }

export interface OpenDirectSaleFormAction { type: 'OPEN_DIRECTSALE_FORM'; }

export interface OpenDirectSaleEditFormAction { type: 'OPEN_DIRECTSALE_EDIT_FORM'; id: string }

export interface cancelDirectSaleFormOpenAction { type: 'CANCEL_DIRECTSALE_FORM_OPEN'; }
export interface cancelSelectedDirectSaleAction { type: 'CANCEL_SELECTED_DIRECTSALE'; }

interface RequestCreateDirectSaleAction { type: 'REQUEST_CREATE_DIRECTSALE'; directsale: DirectSale }

interface ReceiveCreateDirectSaleAction { type: 'RECEIVE_CREATE_DIRECTSALE'; directsale: DirectSale }

interface RequestEditDirectSaleAction { type: 'REQUEST_EDIT_DIRECTSALE'; directsale: DirectSale }
interface ReceiveEditDirectSaleAction { type: 'RECEIVE_EDIT_DIRECTSALE'; directsale: DirectSale }

export interface RequestLoadDirectSaleAction { type: 'REQUEST_LOAD_DIRECTSALE'; id: string }
export interface ReceiveLoadDirectSaleAction { type: 'RECEIVE_LOAD_DIRECTSALE'; directsale: DirectSale }

interface ClearDirectSaleAction { type: 'CLEAR_DIRECTSALE' }


// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestDirectSalesAction | ReceiveDirectSalesAction | SelectDirectSaleAction | RequestCreateDirectSaleAction | ReceiveCreateDirectSaleAction | OpenDirectSaleFormAction | RequestEditDirectSaleAction
    | ReceiveEditDirectSaleAction | OpenDirectSaleEditFormAction | cancelDirectSaleFormOpenAction | cancelSelectedDirectSaleAction | RequestLoadDirectSaleAction | ReceiveLoadDirectSaleAction | ClearDirectSaleAction; //imam

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestDirectSales: (): AppThunkAction<KnownAction> => (
        dispatch,
        getState
    ) => {
        // Only load data if it's something we don't already have (and are not already loading)
        console.log('use effect actionCreators');
        const appState = getState();
        if (appState && appState.directsalesstate) {
            agent.DirectSales.list()

                .then(response => response as DirectSale[])
                .then(data => {
                    console.log('action->RECEIVE_DIRECTSALES_LIST');
                    data.forEach(ds => {
                        ds.endDate = new Date(ds.endDate);
                    })
                    dispatch({
                        type: "RECEIVE_DIRECTSALES_LIST",
                        directsales: data
                    });
                }).catch(error => {
                    console.log(error);
                })
            console.log('action->REQUEST_DIRECTSALES_LIST');
            dispatch({
                type: "REQUEST_DIRECTSALES_LIST"
            });
        }
    },
    selectDirectSale: (id: String) => ({ type: 'SELECT_DIRECTSALE', id: id } as SelectDirectSaleAction),
    openDirectSaleForm: () => ({ type: 'OPEN_DIRECTSALE_FORM' } as OpenDirectSaleFormAction),
    openDirectSaleEditForm: (id: String) => ({ type: 'OPEN_DIRECTSALE_EDIT_FORM', id: id } as OpenDirectSaleEditFormAction),
    cancelDirectSaleFormOpen: () => ({ type: 'CANCEL_DIRECTSALE_FORM_OPEN' } as cancelDirectSaleFormOpenAction),
    cancelSelectedDirectSale: () => ({ type: 'CANCEL_SELECTED_DIRECTSALE' } as cancelSelectedDirectSaleAction),
    createDirectSale: (directsale: DirectSale): AppThunkAction<KnownAction> => (
        dispatch,
        getState
    ) => {
        const appState = getState();
        if (appState && appState.directsalesstate) {
            agent.DirectSales.create(directsale)
                .then(data => {
                    dispatch({
                        type: "RECEIVE_CREATE_DIRECTSALE",
                        directsale: directsale
                    })
                }).catch(error => {
                    console.log(error);
                });
            dispatch({
                type: "REQUEST_CREATE_DIRECTSALE",
                directsale: directsale
            });
        }
    },
    editDirectSale: (directsale: DirectSale): AppThunkAction<KnownAction> => (
        dispatch,
        getState
    ) => {
        const appState = getState();
        if (appState && appState.directsalesstate) {
            agent.DirectSales.update(directsale)
                .then(data => {
                    dispatch({
                        type: "RECEIVE_EDIT_DIRECTSALE",
                        directsale: directsale
                    })
                });
            dispatch({
                type: "REQUEST_EDIT_DIRECTSALE",
                directsale: directsale
            });
        }
    },
    loadDirectSale: (id: string): AppThunkAction<KnownAction> => (
        dispatch,
        getState
    ) => {
        const appState = getState();
        if (appState && appState.directsalesstate) {
            console.log('action REQUEST_LOAD_DIRECTSALE');
            dispatch({
                type: "REQUEST_LOAD_DIRECTSALE",
                id: id
            });
            if (appState.directsalesstate.directsales.find(ds => ds.id === id) === undefined) {
                agent.DirectSales.details(id)
                    .then(response => response as DirectSale)
                    .then(data => {
                        data.endDate = new Date(data.endDate);
                        console.log('action db RECEIVE_LOAD_DIRECTSALE');
                        dispatch({
                            type: "RECEIVE_LOAD_DIRECTSALE",
                            directsale: data
                        });
                    }).catch(error => {
                        console.log(error);
                    });
            } else {
                console.log('action no db RECEIVE_LOAD_DIRECTSALE');
                dispatch({
                    type: "RECEIVE_LOAD_DIRECTSALE",
                    directsale: appState.directsalesstate.directsales.find(ds => ds.id === id)!
                });
            }
        }
    },
    clearDirectSale: () => ({ type: 'CLEAR_DIRECTSALE' } as ClearDirectSaleAction),
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: DirectSaleState = {
    directsales: [],
    isLoading: false,
    directsale: null,
    editMode: false,
    submitting: false
};

export const reducer: Reducer<DirectSaleState> = (
    state: DirectSaleState | undefined,
    incomingAction: Action
): DirectSaleState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_DIRECTSALES_LIST":
            console.log('REQUEST_DIRECTSALES_LIST');
            return {
                ...unloadedState,
                directsales: state.directsales,
                isLoading: true
            };
        case "RECEIVE_DIRECTSALES_LIST":
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            //if (action.directsales.length === state.directsales.length) {
            console.log('RECEIVE_DIRECTSALES_LIST');
            return {
                ...unloadedState,
                directsales: action.directsales,
                isLoading: false
            };
            //}

            break;
        case "SELECT_DIRECTSALE":
            console.log(action.id);
            return {
                ...state,
                editMode: false,
                directsale: state.directsales.find(ds => ds.id === action.id) ?? null
            }
        case "OPEN_DIRECTSALE_FORM":
            return {
                ...state,
                editMode: true,
                directsale: null
            }
        case "CANCEL_DIRECTSALE_FORM_OPEN":
            return {
                ...state,
                editMode: false,
            }
        case "CANCEL_SELECTED_DIRECTSALE":
            return {
                ...state,
                directsale: null
            }

        case "OPEN_DIRECTSALE_EDIT_FORM":
            console.log(action.id);
            return {
                ...state,
                editMode: true,
                directsale: state.directsales.find(ds => ds.id === action.id) ?? null
            }
        case "REQUEST_CREATE_DIRECTSALE":
            console.log('REQUEST_CREATE_DIRECTSALE');
            return {
                ...unloadedState,
                directsales: state.directsales,
                submitting: true
            };
        case "RECEIVE_CREATE_DIRECTSALE":
            console.log('RECEIVE_CREATE_DIRECTSALE');
            state.directsales.push(action.directsale);
            return {
                ...unloadedState,
                directsales: state.directsales,
                submitting: false,
                editMode: false

            };
        case "REQUEST_EDIT_DIRECTSALE":
            console.log('REQUEST_EDIT_DIRECTSALE');
            return {
                ...unloadedState,
                directsales: state.directsales,
                submitting: true
            };
        case "RECEIVE_EDIT_DIRECTSALE":
            console.log('RECEIVE_EDIT_DIRECTSALE');
            return {
                ...unloadedState,
                directsales: [...state.directsales.filter(ds => ds.id !== action.directsale.id), action.directsale],
                directsale: action.directsale,
                submitting: false,
                editMode: false

            };
        case "REQUEST_LOAD_DIRECTSALE":
            console.log('Reducer REQUEST_LOAD_DIRECTSALE');
            return {
                ...state,
                isLoading: true
            }
        case "RECEIVE_LOAD_DIRECTSALE":
            console.log('Reducer RECEIVE_LOAD_DIRECTSALE');
            return {
                ...state,
                directsale: action.directsale,
                isLoading: false
            }
            break;
        case "CLEAR_DIRECTSALE":
            return {
                ...state,
                directsale: null
            }
    }

    return state||unloadedState ;

};
