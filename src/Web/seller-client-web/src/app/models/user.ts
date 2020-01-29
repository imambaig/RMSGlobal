export type User = {
    username: string;
    displayName: string;
    token: string;
    image?: string;
}

export type UserFormValues = {
    emai: string;
    password: string;
    displayName?: string;
    username?: string;
}