import { Component, input } from '@angular/core';

/**
 * Prazno stanje: kaže šta nedostaje i šta korisnik može uraditi.
 *
 *   <app-empty-state icon="route" [title]="'TOURS.EMPTY_TITLE' | translate" [description]="...">
 *     <button mat-flat-button>Dodaj turu</button>   (neobavezno dugme)
 *   </app-empty-state>
 */
@Component({
  selector: 'app-empty-state',
  standalone: false,
  templateUrl: './empty-state.component.html',
  styleUrl: './empty-state.component.scss'
})
export class EmptyStateComponent {
  readonly icon = input('inbox');
  readonly title = input.required<string>();
  readonly description = input<string>();
}