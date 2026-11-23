import { Component } from '@angular/core';
import { NavSection } from '../../shared/models/nav-item.model';

/** Layout admina platforme: stavke menija, a izgled je u DashboardShellComponent. */
@Component({
  selector: 'app-admin-layout',
  standalone: false,
  templateUrl: './admin-layout.component.html'
})
export class AdminLayoutComponent {
  readonly sections: NavSection[] = [
    {
      titleKey: 'ADMIN_PANEL.MENU.SYSTEM',
      items: [
        { icon: 'currency_exchange', labelKey: 'ADMIN_PANEL.MENU.EXCHANGE_RATES', route: '/admin/exchange-rates' },
        { icon: 'settings', labelKey: 'ADMIN_PANEL.MENU.SETTINGS', route: '/admin/settings' },
        { icon: 'manage_accounts', labelKey: 'ADMIN_PANEL.MENU.MY_PROFILE', route: '/admin/profile' }
      ]
    }
  ];
}