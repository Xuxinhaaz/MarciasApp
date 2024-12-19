import { Injectable } from '@angular/core';
import { Food } from '../model/Food';
import { RecipesList } from '../Recipes';
@Injectable({
  providedIn: 'root'
})
export class FoodService {

  constructor() { }
  getAll():Food[]{
    return RecipesList;
  }

  getFoodById(foodId:string){
    return  this.getAll().find(food => food.id.toString() == foodId)?? new Food();
    }
}
