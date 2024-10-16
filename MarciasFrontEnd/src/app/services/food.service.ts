import { Injectable } from '@angular/core';
import { Food } from '../Components/model/Foods';
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
    return  this.getAll().find(food => food.id)?? new Food();
    }
}
