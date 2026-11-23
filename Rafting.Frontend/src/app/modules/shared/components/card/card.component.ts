import { Component, input } from '@angular/core';

/**
 * Kartica: ravna površina s naslovom, neobaveznim opisom i akcijama desno.
 *
 *   <app-card [title]="'ORDERS.TITLE' | translate" [description]="...">
 *     <button cardActions mat-stroked-button>Dodaj</button>
 *     ...sadržaj kartice...
 *   </app-card>
 */
@Component({
  selector: 'app-card',
  standalone: false,
  templateUrl: './card.component.html',
  styleUrl: './card.component.scss'
})
export class CardComponent {
  readonly title = input.required<string>();
  readonly description = input<string>();
}