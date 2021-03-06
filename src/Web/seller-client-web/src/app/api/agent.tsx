﻿import axios, { AxiosResponse } from 'axios'
import { DirectSale } from '../models/directsale';
import { User,UserFormValues } from '../models/user';
import { history} from '../..'
import { toast } from 'react-toastify';

axios.defaults.baseURL = 'http://localhost:9001';

axios.interceptors.request.use(
    config => {
        const token = window.localStorage.getItem("jwt");
        if (token) config.headers.Authorization = `Bearer ${token}`;
        return config;
    },
    error => {
        return Promise.reject(error);
    }
);

axios.interceptors.response.use(undefined, error => {
    if (error.message === "Network Error" && !error.response) {
        toast.error("Network error - make sure API is running!");
    }
    const {status,data,config } = error.response;
    console.log(error.response);
    if (status === 404) {
        history.push('/notfound');
    }
    if (status === 400 && config.method === 'get' && data.errors.hasOwnProperty('id')) {
        history.push('/notfound');
    }
    if (status === 500) {
        toast.error('server error -- check the terminal for more info!')
    }

    throw error.response;
})

const responseBody = (response: AxiosResponse) => response.data;

const sleep = (ms: number) => (respose: AxiosResponse) =>
    new Promise<AxiosResponse>(resolve => setTimeout(() => resolve(respose), ms));

const requests = {
    get: (url: string) => axios.get(url).then(sleep(1000)).then(responseBody),
    //get: (url: string) => axios.get(url).then(responseBody),
    post: (url: string, body: {}) => axios.post(url, body).then(sleep(1000)).then(responseBody),
    put: (url: string, body: {}) => axios.put(url, body).then(sleep(1000)).then(responseBody),
    del: (url: string) => axios.delete(url).then(sleep(1000)).then(responseBody)
}

const DirectSales = {
   list:():Promise<DirectSale[]> =>requests.get('/directsales'),
    details: (id: string) => requests.get(`/directsales/${id}`),
    create: (directsale: DirectSale) => requests.post(`/directsales/`, directsale),
    update: (directsale: DirectSale) =>
        requests.put(`/directsales/${directsale.id}`, directsale),
    delete: (id: string) => requests.del(`/directsales/${id}`)
};

const Users = {
    current: (): Promise<User> => requests.get('/user'),
    login: (user: UserFormValues): Promise<User> => requests.post(`/user/login`, user),
    register: (user: UserFormValues): Promise<User> => requests.post('/user/register',user)
}

export default {
    DirectSales,
    Users
}