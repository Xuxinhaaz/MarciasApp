import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-mkt-header',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './mkt-header.component.html',
  styleUrl: './mkt-header.component.scss'
})
export class MktHeaderComponent {

}
