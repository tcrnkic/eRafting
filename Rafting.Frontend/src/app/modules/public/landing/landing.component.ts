import { Component, inject } from '@angular/core';
import { BaseComponent } from '../../../core/components/base-classes/base-component';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';

/**
 * Početna (landing) stranica javnog dijela aplikacije.
 * Ruta: '' unutar PublicLayoutComponent (vidi public-routing-module.ts).
 */
@Component({
  selector: 'app-landing',
  standalone: false,
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.scss',
})
export class LandingComponent extends BaseComponent {
  protected currentUser = inject(CurrentUserService);
}