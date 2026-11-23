import { Component } from '@angular/core';
import { inject, signal } from '@angular/core';           // spoji s postojećim importom iz '@angular/core'
import { TranslateService } from '@ngx-translate/core';
import { DialogHelperService } from '../../shared/services/dialog-helper.service';
import { CalendarDay } from '../../shared/models/calendar-model';
import { toDateKey } from '../../shared/components/calendar-view/calendar-grid';

/** Početna stranica vlasnika. Privremena: pravi pregled dolazi u zadatku B13. */

@Component({
  selector: 'app-owner-home',
  standalone: false,
  templateUrl: './owner-home.component.html',
  styleUrl: './owner-home.component.scss'
  
})

export class OwnerHomeComponent {

  // unutar klase:
  private readonly dialogs = inject(DialogHelperService);
  private readonly translate = inject(TranslateService);

  protected readonly selectedDay = signal<string | null>(null);
  protected readonly demoResult = signal<string | null>(null);

  protected readonly demoDays: CalendarDay[] = this.buildDemoDays();

  protected onDayClick(date: string): void {
    this.selectedDay.set(date);
  }

  protected cancelDemo(): void {
    const date = this.selectedDay() ?? toDateKey(new Date());
    this.dialogs
      .confirmIrreversible('OWNER.DEMO.CANCEL_TITLE', 'OWNER.DEMO.CANCEL_MESSAGE', 'OWNER.DEMO.CANCEL_CONFIRM', { date })
      .subscribe(confirmed =>
        this.demoResult.set(this.translate.instant(confirmed ? 'OWNER.DEMO.CONFIRMED' : 'OWNER.DEMO.ABORTED'))
      );
  }

  private buildDemoDays(): CalendarDay[] {
    const now = new Date();
    const key = (day: number) => toDateKey(new Date(now.getFullYear(), now.getMonth(), day));
    return [
      { date: key(4), condition: 'good', events: [{ title: 'Una rafting', time: '10:00' }] },
      { date: key(9), condition: 'caution', events: [{ title: 'Una rafting', time: '10:00' }, { title: 'Porodična tura', time: '14:00' }] },
      { date: key(15), condition: 'closed' },
      { date: key(21), condition: 'good', events: [{ title: 'Una rafting', time: '09:00' }, { title: 'Una rafting', time: '12:00' }, { title: 'Večernja tura', time: '17:00' }] }
    ];
  }
}