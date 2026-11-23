export type DayCondition = 'good' | 'caution' | 'closed';

export interface CalendarEvent {
  title: string;
  time?: string;
}

export interface CalendarDay {
  /** Format 'yyyy-MM-dd'. */
  date: string;
  /** Uslovi na rijeci. Ako nije zadano, dan se prikazuje neutralno. */
  condition?: DayCondition;
  events?: CalendarEvent[];
}