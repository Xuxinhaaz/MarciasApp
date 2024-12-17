import { Food } from "./Food";

export class CartItem{
    constructor(public food:Food){
        this.price = food.price;
    }
    quantity:number = 1;
    price:number;
}