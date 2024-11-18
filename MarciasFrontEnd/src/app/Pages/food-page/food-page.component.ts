import { Component, OnInit } from '@angular/core';
import { FoodService } from '../../services/food.service';
import { Food } from '../../Components/model/Foods';
import { ActivatedRoute } from '@angular/router';
import { MktHeaderComponent } from "../../Components/mktHeader/mkt-header/mkt-header.component";
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-food-page',
  standalone: true,
  imports: [MktHeaderComponent, NgFor],
  templateUrl: './food-page.component.html',
  styleUrl: './food-page.component.scss'
})
export class FoodPageComponent implements OnInit {
food!:Food;
  constructor(activatedRoute:ActivatedRoute, private foodService:FoodService){
    activatedRoute.params.subscribe((params)=>{
      if(params['id'])
      this.food = this.foodService.getFoodById(params['id']);
    })
  }
  ngOnInit(): void {
    
  }
}
