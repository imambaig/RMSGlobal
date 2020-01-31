import * as DirectSales from "../reducers/DirectSalesReducer";
import * as Users from "../reducers/UsersReducer";
import * as Modal from "../reducers/ModalStore";
//export { StoreProvider } from "./provider";

// The top-level state object
export interface ApplicationState {
    directsalesstate: DirectSales.DirectSaleState | undefined;
    usersstate: Users.UserState | undefined;
    modalstate: Modal.ModalState | undefined;
}

/*
export const initialApplicationState: ApplicationState={
    directsalesstate: undefined,
    usersstate: undefined,
    modalstate
}*/
// Whenever an action is dispatched, Redux will update each top-level application state property using
// the reducer with the matching name. It's important that the names match exactly, and that the reducer
// acts on the corresponding ApplicationState property type.
export const reducers = {
    directsalesstate: DirectSales.reducer,
    usersstate: Users.reducer,
    modalstate:Modal.reducer
};

// This type can be used as a hint on action creators so that its 'dispatch' and 'getState' params are
// correctly typed to match your store.
export interface AppThunkAction<TAction> {
    (dispatch: (action: TAction) => void, getState: () => ApplicationState): void;
}


