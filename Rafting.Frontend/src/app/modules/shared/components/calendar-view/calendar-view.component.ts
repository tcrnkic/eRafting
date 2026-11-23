import { Component, LOCALE_ID, computed, inject, input, linkedSignal, output } from '@angular/core';
import { formatDate } from '@angular/common';
import { TranslateService } from '@ngx-translate/core';
import { CalendarDay } from '../../models/calendar-model';
import { GridCell, buildMonthGrid } from './calendar-grid';

/**
 * Mjesečni kalendar s bojama dana prema uslovima na rijeci.
 *
 *   <app-calendar-view [days]="days" (dayClick)="open($event)" (monthChange)="load($event)" />
 *
 * Komponenta ne učitava podatke. Roditelj daje `days` i sluša `monthChange`.
 */
@Component({
  selector: 'app-calendar-view',
  standalone: false,
  templateUrl: './calendar-view.component.html',
  styleUrl: './calendar-view.component.scss'
})
export class CalendarViewComponent {
  private readonly locale = inject(LOCALE_ID);
  private readonly translate = inject(TranslateService);

  /** Mjesec koji se prikazuje na početku (bilo koji dan u tom mjesecu). */
  readonly month = input<Date>(new Date());
  readonly days = input<CalendarDay[]>([]);

  /** Emituje 'yyyy-MM-dd' kliknutog dana. */
  readonly dayClick = output<string>();
  /** Emituje prvi dan novog mjeseca kad korisnik promijeni mjesec. */
  readonly monthChange = output<Date>();

  /** Mjesec koji korisnik trenutno gleda. Resetuje se kad roditelj promijeni `month`. */
  protected readonly viewMonth = linkedSignal(() => {
    const m = this.month();
    return new Date(m.getFullYear(), m.getMonth(), 1);
  });

  protected readonly weeks = computed(() => {
    const m = this.viewMonth();
    return buildMonthGrid(m.getFullYear(), m.getMonth(), new Date());
  });

  protected readonly dayMap = computed(() => new Map(this.days().map(d => [d.date, d])));

  /** 1. januar 2024. je ponedjeljak, pa dobijamo zaglavlja pon-ned. */
  protected readonly weekdays = Array.from({ length: 7 }, (_, i) => new Date(2024, 0, 1 + i));

  protected shift(delta: number): void {
    const m = this.viewMonth();
    this.setMonth(new Date(m.getFullYear(), m.getMonth() + delta, 1));
  }

  protected goToday(): void {
    const t = new Date();
    this.setMonth(new Date(t.getFullYear(), t.getMonth(), 1));
  }

  /** Opis dana za čitače ekrana: puni datum, uslovi i broj termina. */
  protected ariaLabel(cell: GridCell, day: CalendarDay | undefined): string {
    const parts = [formatDate(cell.date, 'fullDate', this.locale)];
    if (day?.condition) {
      parts.push(this.translate.instant('CALENDAR.CONDITION.' + day.condition.toUpperCase()));
    }
    return parts.join(', ');
  }

  private setMonth(next: Date): void {
    this.viewMonth.set(next);
    this.monthChange.emit(next);
  }
}