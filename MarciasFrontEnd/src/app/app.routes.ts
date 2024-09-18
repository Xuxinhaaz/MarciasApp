import { Routes, ExtraOptions } from '@angular/router';
import { HomeComponent } from './Pages/StartingPage/home/home.component';
import { LoginComponent } from './Pages/StartingPage/login/login.component';
import { MarketHomeComponent } from "./Pages/MarketPage/market-home/market-home.component";

export const routes: Routes = [
    {
        path: '',
        component: HomeComponent,
    }
    ,
    {
        path: 'login',
        component: LoginComponent,
    },
    {
        path: 'market', 
        component: MarketHomeComponent,
    },
];