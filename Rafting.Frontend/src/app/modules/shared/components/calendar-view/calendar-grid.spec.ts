import { buildMonthGrid, toDateKey } from './calendar-grid';

describe('toDateKey', () => {
  it('formatira lokalni datum kao yyyy-MM-dd s vodećim nulama', () => {
    expect(toDateKey(new Date(2026, 0, 5))).toBe('2026-01-05');
  });

  it('ne pomjera dan kasno uvečer (nema pretvaranja u UTC)', () => {
    expect(toDateKey(new Date(2026, 10, 9, 23, 59))).toBe('2026-11-09');
  });
});

describe('buildMonthGrid', () => {
  const today = new Date(2026, 10, 23);

  it('novembar 2026 (počinje u nedjelju) ima 6 sedmica po 7 dana', () => {
    const weeks = buildMonthGrid(2026, 10, today);
    expect(weeks.length).toBe(6);
    expect(weeks.every(w => w.length === 7)).toBe(true);
  });

  it('sedmica počinje ponedjeljkom: prva ćelija je 26.10., zadnja 6.12.', () => {
    const cells = buildMonthGrid(2026, 10, today).flat();
    expect(cells[0].key).toBe('2026-10-26');
    expect(cells[cells.length - 1].key).toBe('2026-12-06');
  });

  it('februar 2027 (počinje u ponedjeljak, 28 dana) ima tačno 4 sedmice', () => {
    const weeks = buildMonthGrid(2027, 1, today);
    expect(weeks.length).toBe(4);
    expect(weeks[0][0].key).toBe('2027-02-01');
    expect(weeks[3][6].key).toBe('2027-02-28');
  });

  it('inMonth je true samo za dane tog mjeseca', () => {
    const cells = buildMonthGrid(2026, 10, today).flat();
    expect(cells.filter(c => c.inMonth).length).toBe(30);
    expect(cells[0].inMonth).toBe(false);
  });

  it('tačno jedna ćelija je današnja, i to ona s ispravnim datumom', () => {
    const cells = buildMonthGrid(2026, 10, today).flat();
    const todays = cells.filter(c => c.isToday);
    expect(todays.length).toBe(1);
    expect(todays[0].key).toBe('2026-11-23');
  });

  it('prelazak godine: januar 2027 počinje u petak, pa prva ćelija je 28.12.2026.', () => {
    const cells = buildMonthGrid(2027, 0, today).flat();
    expect(cells[0].key).toBe('2026-12-28');
  });
});