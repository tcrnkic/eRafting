export interface GridCell {
  date: Date;
  /** 'yyyy-MM-dd' */
  key: string;
  inMonth: boolean;
  isToday: boolean;
}

/** Lokalni datum kao 'yyyy-MM-dd'. Ne koristi toISOString jer on pretvara u UTC i može pomjeriti dan. */
export function toDateKey(date: Date): string {
  const y = date.getFullYear();
  const m = String(date.getMonth() + 1).padStart(2, '0');
  const d = String(date.getDate()).padStart(2, '0');
  return `${y}-${m}-${d}`;
}

/**
 * Mreža mjeseca po sedmicama. Sedmica počinje ponedjeljkom.
 * month je 0-11 (kao u JavaScript Date).
 */
export function buildMonthGrid(year: number, month: number, today: Date): GridCell[][] {
  const offset = (new Date(year, month, 1).getDay() + 6) % 7; // ponedjeljak = 0
  const daysInMonth = new Date(year, month + 1, 0).getDate();
  const weekCount = Math.ceil((offset + daysInMonth) / 7);
  const todayKey = toDateKey(today);

  const weeks: GridCell[][] = [];
  for (let w = 0; w < weekCount; w++) {
    const week: GridCell[] = [];
    for (let d = 0; d < 7; d++) {
      const date = new Date(year, month, 1 - offset + w * 7 + d);
      const key = toDateKey(date);
      week.push({ date, key, inMonth: date.getMonth() === month, isToday: key === todayKey });
    }
    weeks.push(week);
  }
  return weeks;
}