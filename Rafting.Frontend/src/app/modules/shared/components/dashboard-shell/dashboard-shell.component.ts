import { BreakpointObserver } from '@angular/cdk/layout';
import { Component, Input, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { TranslateService } from '@ngx-translate/core';
import { map } from 'rxjs';
import { AuthFacadeService } from '../../../../core/services/auth/auth-facade.service';
import { NavSection } from '../../models/nav-item.model';

/**
 * Zajednički okvir za sve nadzorne ploče (admin, vlasnik agencije, skiper):
 * bočni meni, podaci o korisniku, izbor jezika i odjava. Stranica se ubacuje kao sadržaj:
 *
 *   <app-dashboard-shell [sections]="sections" roleKey="ADMIN_PANEL.ROLES.ADMIN">
 *     <router-outlet />
 *   </app-dashboard-shell>
 *
 * Na ekranu užem od 900 px meni se krije i otvara dugmetom u gornjoj traci.
 */
@Component({
  selector: 'app-dashboard-shell',
  standalone: false,
  templateUrl: './dashboard-shell.component.html',
  styleUrl: './dashboard-shell.component.scss'
})
export class DashboardShellComponent {
  private translate = inject(TranslateService);
  private breakpoints = inject(BreakpointObserver);
  protected auth = inject(AuthFacadeService);

  /** Stavke menija, grupisane u sekcije. */
  @Input({ required: true }) sections!: NavSection[];

  /** Ključ prijevoda za naziv uloge koji se vidi ispod emaila. */
  @Input({ required: true }) roleKey!: string;

  protected readonly compact = toSignal(
    this.breakpoints.observe('(max-width: 900px)').pipe(map(state => state.matches)),
    { initialValue: false }
  );
  protected readonly menuOpen = signal(false);

  protected readonly languages = [
    { code: 'bs', name: 'Bosanski' },
    { code: 'en', name: 'English' }
  ];
  protected currentLang = this.translate.currentLang || 'bs';

  protected switchLanguage(code: string): void {
    this.currentLang = code;
    this.translate.use(code);
    localStorage.setItem('language', code);
  }

  protected get currentLanguageName(): string {
    return this.languages.find(l => l.code === this.currentLang)?.name ?? '';
  }
}