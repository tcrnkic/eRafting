import { Component } from '@angular/core';
import { NavSection } from '../../shared/models/nav-item.model';

/** Layout skipera: stavke menija, a izgled je u DashboardShellComponent. */
@Component({
  selector: 'app-skiper-layout',
  standalone: false,
  templateUrl: './skiper-layout.component.html'
})
export class SkiperLayoutComponent {
  readonly sections: NavSection[] = [
    {
      items: [
        { icon: 'dashboard', labelKey: 'SKIPER.MENU.OVERVIEW', route: '/skiper' },
        { icon: 'manage_accounts', labelKey: 'ADMIN_PANEL.MENU.MY_PROFILE', route: '/skiper/profile' }
      ]
    }
  ];
}