// src/app/core/services/auth/current-user.service.ts
import { Injectable, inject, computed } from '@angular/core';
import { AuthFacadeService } from './auth-facade.service';

@Injectable({ providedIn: 'root' })
export class CurrentUserService {
  private auth = inject(AuthFacadeService);

  /** Signal koji UI može čitati (readonly) */
  currentUser = computed(() => this.auth.currentUser());

  isAuthenticated = computed(() => this.auth.isAuthenticated());
  isAdmin = computed(() => this.auth.isAdmin());
  isManager = computed(() => this.auth.isManager());
  isEmployee = computed(() => this.auth.isEmployee());

  get snapshot() {
    return this.auth.currentUser();
  }

  /** Početna stranica po ulozi: admin > vlasnik > skiper > gost */
  getDefaultRoute(): string {
    const user = this.snapshot;
    if (!user) return '/auth/login';

    if (user.isAdmin) return '/admin';
    if (user.isManager) return '/owner';
    if (user.isEmployee) return '/skiper';
    return '/';
  }
}
