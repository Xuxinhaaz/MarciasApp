import { Component, ElementRef, viewChild } from '@angular/core';
import { MktHeaderComponent } from "../../../Components/mktHeader/mkt-header/mkt-header.component";
import { FoodsComponent } from '../../../Components/foods/foods.component';
import { ViewportScroller } from '@angular/common';
import { RecipesList } from '../../../Recipes';
import { Router } from '@angular/router';

@Component({
  selector: 'app-market-home',
  standalone: true,
  imports: [MktHeaderComponent, FoodsComponent],
  templateUrl: './market-home.component.html',
  styleUrl: './market-home.component.scss'
})

export class MarketHomeComponent {
  foods= RecipesList;
}

