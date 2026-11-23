import { Component } from '@angular/core';
import { NavSection } from '../../shared/models/nav-item.model';

/** Layout vlasnika agencije: stavke menija, a izgled je u DashboardShellComponent. */
@Component({
  selector: 'app-owner-layout',
  standalone: false,
  templateUrl: './owner-layout.component.html'
})
export class OwnerLayoutComponent {
  readonly sections: NavSection[] = [
    {
      items: [
        { icon: 'dashboard', labelKey: 'OWNER.MENU.OVERVIEW', route: '/owner' },
        { icon: 'manage_accounts', labelKey: 'ADMIN_PANEL.MENU.MY_PROFILE', route: '/owner/profile' }
      ]
    }
  ];
}