import { Dispatch } from 'redux';
import { ActionTypes } from './DSActionTypes';
import { DirectSale } from "../models/directsale";
import agent from "../api/agent";

export interface RequestDirectSaleList {
    type: ActionTypes.REQUEST_DIRECTSALES_LIST;
    payload: DirectSale[];
}

export interface DeleteTodoAction {
  type: ActionTypes.deleteTodo;
  payload: number;
}
export const fetchTodos = () => {
  return async (dispatch: Dispatch) => {
      agent.DirectSales.list()
          .then(response => response as DirectSale[])
          .then(data => {
              dispatch({
                  type: ActionTypes.REQUEST_DIRECTSALES_LIST,
                  payload: data
              });
          });
      
  };
};

export const deleteTodo = (id: number): DeleteTodoAction => {
  return {
    type: ActionTypes.deleteTodo,
    payload: id
  };
};
