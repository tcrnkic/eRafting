/** Jedna stavka bočnog menija. labelKey je ključ prijevoda (ngx-translate). */
export interface NavItem {
  icon: string;
  labelKey: string;
  route: string;
}

/** Grupa stavki s neobaveznim naslovom. */
export interface NavSection {
  titleKey?: string;
  items: NavItem[];
}