import { Customer } from "./customer";

export class Order {
    id: string = '';
    name: string = '';
    description: string = '';
    totalAmount?: number;
    customer: Customer = new Customer();
}