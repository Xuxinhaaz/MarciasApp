import { Component, OnInit } from '@angular/core';
import { FoodService } from '../../services/food.service';
import { Food } from '../../Components/model/Foods';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-food-page',
  standalone: true,
  imports: [],
  templateUrl: './food-page.component.html',
  styleUrl: './food-page.component.scss'
})
export class FoodPageComponent implements OnInit {

  constructor(activatedRoute:ActivatedRoute, private foodService:FoodService){}
  ngOnInit(): void {
    
  }
}
