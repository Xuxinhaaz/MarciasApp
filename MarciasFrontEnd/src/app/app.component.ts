import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./Components/header/header.component";
import { HomeComponent } from "./Pages/StartingPage/home/home.component";
import { LoginComponent } from "./Pages/StartingPage/login/login.component";
import { RecipesList } from "./Recipes";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HomeComponent, LoginComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'MarciasProject';
}
