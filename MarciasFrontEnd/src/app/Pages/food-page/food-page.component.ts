import { Component, OnInit } from '@angular/core';
import { FoodService } from '../../services/food.service';
import { Food } from '../../model/Food';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MktHeaderComponent } from "../../Components/mktHeader/mkt-header/mkt-header.component";
import { NgFor } from '@angular/common';
import { CartService } from '../../services/cart.service';

@Component({
  selector: 'app-food-page',
  standalone: true,
  imports: [MktHeaderComponent, NgFor, RouterLink ],
  templateUrl: './food-page.component.html',
  styleUrl: './food-page.component.scss'
})
export class FoodPageComponent implements OnInit {
  food!: Food;
  constructor(activatedRoute:ActivatedRoute, foodService:FoodService,
    private cartService:CartService, private router: Router) {
    activatedRoute.params.subscribe((params) => {
      if(params["id"])
        this.food = foodService.getFoodById(params["id"]);
    })
  }
  addToCart(){
    this.cartService.addToCart(this.food);
    this.router.navigateByUrl('/cart');
  }
  ngOnInit(): void {
    
  }
}
