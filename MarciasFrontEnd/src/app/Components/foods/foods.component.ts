import { Component, computed, Input, input } from '@angular/core';

type Food = {
  id:string;
  name:string;
  price:number;
  compliment: string;
  img: string;
}

@Component({
  selector: 'food',
  standalone: true,
  imports: [],
  templateUrl: './foods.component.html',
  styleUrl: './foods.component.scss'
})
export class FoodsComponent {
  @Input ({required:true}) foodItem!: Food;
   get imagePath(){
    return 'foodsImg/' + this.foodItem.img;
  }
}
