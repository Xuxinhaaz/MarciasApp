import { Component } from '@angular/core';
import { Cart } from '../../model/Cart';
import { CartItem } from '../../model/CartItem';
import { Food } from '../../model/Food';
import { CartService } from '../../services/cart.service';
import { MktHeaderComponent } from '../../Components/mktHeader/mkt-header/mkt-header.component';
import { OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule, NgFor } from '@angular/common';

@Component({
  selector: 'app-cart-page',
  standalone: true,
  imports: [MktHeaderComponent, RouterLink, NgFor, CommonModule],
  templateUrl: './cart-page.component.html',
  styleUrl: './cart-page.component.scss'
})
export class CartPageComponent implements OnInit {
  cart!: Cart;
  constructor(private cartService: CartService) {
    this.cartService.getCartObservable().subscribe((cart) => {
      this.cart = cart;
    })
   }

  ngOnInit(): void {
  }

  removeFromCart(cartItem:CartItem){
    this.cartService.removeFromCart(cartItem.food.id.toString());
  }

  changeQuantity(cartItem:CartItem,quantityInString:string){
    const quantity = parseInt(quantityInString);
    this.cartService.changeQuantity(cartItem.food.id.toString(), quantity);
  }

}