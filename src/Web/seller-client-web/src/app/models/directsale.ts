export type DirectSale={
    id: string;
    name: string,
    endDate: Date;
    directSaleType:string

}

export type DiretSaleFormValues = Partial<DirectSale> &
{
    time?:Date
}

export class DiretSaleFormValue implements DiretSaleFormValues {
    id?: string=undefined;
    name: string = '';
    endDate?: Date=undefined;
    time?: Date = undefined;
    directSaleType: string = '';
    constructor(init?: DiretSaleFormValues) {
        if (init && init.endDate) {
            init.time=init.endDate
        }
        Object.assign(this, init); // assign properties
        console.log(this);
    }
}