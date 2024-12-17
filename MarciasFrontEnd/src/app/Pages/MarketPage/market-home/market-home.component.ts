import { Food } from '../../../model/Food';
import { Component, ElementRef, viewChild } from '@angular/core';
import { MktHeaderComponent } from "../../../Components/mktHeader/mkt-header/mkt-header.component";
import { NgFor, ViewportScroller } from '@angular/common';
import { RecipesList } from '../../../Recipes';
import { Router, RouterLink } from '@angular/router';
import { FoodService } from '../../../services/food.service';

@Component({
  selector: 'app-market-home',
  standalone: true,
  imports: [MktHeaderComponent, NgFor, RouterLink],
  templateUrl: './market-home.component.html',
  styleUrl: './market-home.component.scss'
})

export class MarketHomeComponent {
foods:Food[] = [];
  constructor(private api:FoodService){
    this.foods = api.getAll();
  }
}


