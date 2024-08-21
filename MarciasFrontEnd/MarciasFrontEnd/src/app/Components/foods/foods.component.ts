import { Component, Input } from '@angular/core';
import { RecipesList } from "../../Recipes";

@Component({
  selector: 'food',
  standalone: true,
  imports: [],
  templateUrl: './foods.component.html',
  styleUrl: './foods.component.scss'
})
export class FoodsComponent {
  @Input() food!: string;
  selectedFood= RecipesList[0];
}
