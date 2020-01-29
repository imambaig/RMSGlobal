import { DeleteTodoAction, RequestDirectSaleList } from './DSActions';

export enum ActionTypes {
    REQUEST_DIRECTSALES_LIST,
    deleteTodo
}

export type Action = RequestDirectSaleList | DeleteTodoAction;
// this along with the enum
//sets up an implicit type guard in the reducer