# eRafting: kontekst projekta za Claude agenta

Ovaj fajl je namijenjen Claude agentu koji pomaže jednom od dva studenta. Stavi ga u korijen repozitorija kao `CLAUDE.md` (ili ga zalijepi na početku razgovora). Pročitaj ga cijelog prije bilo kakvog rada.

## Ko i šta
- Predmet: RS I (Razvoj softvera I), 2026/27. Projekat "eRafting", grupa od dva studenta: **Tarik Crnkić (IB240139)** i **Advan Bešić (IB240116)**, oboje redovni. Jezik komunikacije: bosanski.
- Grupa je izuzetak od samostalnog rada; svaki član ima **vlastiti flow** koji samostalno realizuje. Još nije odlučeno ko uzima Flow A, a ko Flow B. Pitaj korisnika na kojem flowu radi prije nego što dotakneš tuđe entitete.
- Ocjenjivanje: rad je prihvaćen kad funkcionalnosti nose min. 40 bodova (10 osnovnih + 30 naprednih). Osnovne max 15 bodova. Odbrana je pisana, bez AI, i čini veći dio ocjene: pitanja se sastavljaju iz implementiranih funkcionalnosti.
- **Zato:** objašnjavaj svaki dio koda koji pišeš. Korisnik mora moći sam obrazložiti svaku klasu, validaciju, migraciju i odluku. Ne piši veliku količinu koda bez objašnjenja. Predlaži male korake i pitaj ako nešto nije jasno. Ne gurni funkcionalnosti izvan backloga.

## Šta je proizvod
Platforma na kojoj agencije za rafting objavljuju ture, a gosti rezervišu termine. Dijelovi: naša landing stranica, super admin panel (admin vidi sve), prijava agencije koju admin odobrava, dashboard vlasnika agencije, dashboard skipera, javna stranica agencije `/a/{slug}` s kalendarom termina, vremenska prognoza kao upozorenje.

## Tehnologije (starter: github.com/adil-fit-ba/rs1-2026-27, folder `starter`)
- Backend: .NET 10 Web API, Clean Architecture (Domain, Application, Infrastructure, API), CQRS (MediatR), FluentValidation, EF Core + SQL Server, JWT + refresh tokeni, `BaseEntity` sa soft delete, `ICurrencyConverter`, `IEmailSender`, `ICacheService`, upload slike.
- Frontend: Angular 21 (NgModules), Angular Material, ngx-translate (bs/en).
- Baza `Market2026`, API `https://localhost:7100`, frontend obično `http://localhost:4200`.
- Struktura repozitorija: `Market.Backend/`, `Market.Frontend/`, `dokumenti/` (dijagrami .png/.jpg, screenshotovi), `db-backups/` (backup baze, zip).
- Kod iz startera se ne boduje; boduje se ono što sami naprave. Ono što eRafting ne koristi iz startera briše se (Product, ProductCategory, Order, OrderItem, Promotion, UserProductFavorite, korpa, checkout).

## Lokalno okruženje
- Treba .NET 10 SDK i **Visual Studio 2026** (ili Rider; VS 2022 ne može buildati .NET 10), Node LTS, Git, Docker Desktop.
- SQL Server u Dockeru: `docker run -d --name erafting-sql -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Test12345!" -p 1433:1433 mcr.microsoft.com/mssql/server:2022-latest`, poslije samo `docker start erafting-sql`. Lozinka `test` iz startera je preslaba za SQL Server, pa u `appsettings.json` i `appsettings.Testing.json` stoji `Password=Test12345!`.
- Backend: `dotnet run --project Market.API` iz `Market.Backend`. Baza se sama kreira i migrira, demo podaci se upisuju u Development.
- Frontend: `npm install`, `npm start` u `Market.Frontend`.
- Repozitorij držati u kratkoj putanji (npr. `C:\rs1\eRafting`) i jednom `git config --global core.longpaths true`.

## Tok rada (obavezno)
- GitHub Project (Kanban, javan) s kolonama Backlog, Ready, In progress, In review, Done; polje Estimate (sati) se upisuje pri prelasku u Ready. Bez sprintova.
- Jedan issue = jedna grana = jedan pull request. Grana: `{broj}-kratak-opis` (npr. `12-rezervacija-potvrda`). U opisu PR-a `Closes #{broj}`.
- Commit poruke sadrže broj issuea: `Potvrda rezervacije (#12)`. Commit-uje se čim je dio zadatka gotov.
- Nikad ne radi direktno na `main`. Prije granjanja `git pull` na `main`. Drugi član pregleda PR.
- Testiranje: ručno i unit testovi nad vlastitom logikom (testovi iz startera se ne broje).
- Svaki novi entitet ide u dijagram klase / ER dijagram u `dokumenti`.
- **EF migracije:** ako se sukobi `MarketDbContextModelSnapshot.cs`, ne rješavaj ručno: obriši svoju migraciju, povuci `main` i napravi je ponovo.
- Backup baze (zip) u `db-backups` prije predaje.

## Pravila domene (odluke tima)
- Multi-tenant preko `AgencyId`: vlasnik i skiper vide samo svoju agenciju (AgencyId iz tokena), gost ulazi preko `/a/{slug}`, admin vidi sve (u agencijama samo čitanje).
- Uloge: `IsAdmin` = admin platforme, `IsManager` = vlasnik agencije, `IsEmployee` = skiper, gost nema zastavicu.
- Gost mora biti registrovan prije rezervacije (email i telefon obavezni). Jedan gost može prijaviti više osoba; učesnici nemaju naloge nego se unose u formu. Telefonske rezervacije nisu u obimu.
- Grupa po čamcu: tvrdi minimum 4, preporučeno 6, maksimum 8. Termin ispod 4 ne može krenuti.
- Termin ostaje otvoren do popunjenosti, zatvaranja, otkazivanja ili isteka roka za rezervacije; gosti vide broj prijavljenih (bez imena).
- Spajanje malih grupa: sistem predlaže, vlasnik potvrđuje, gost prihvata ili odbija. Prozor 3 dana, rok odluke 48 sati prije ranijeg termina. Rezervacija se ne dijeli.
- Rok za rezervacije 24 h prije početka. Razmak za pripremu između tura 0, 15, 30 ili 60 min (vlasnik postavlja).
- Vremenska prognoza (Open-Meteo, keširana) samo upozorava; vlasnik ručno otkazuje termin. Termin otkazan od agencije = puni povrat (`CancelledByAgency`).
- Skiper može odbiti ili otkazati dodjelu samo više od 24 h prije polaska; vlasnik ga može zamijeniti u svakom trenutku.
- Valute KM i EUR. Uz uplatu: valuta, kurs, datum kursa, iznos u KM. Valuta se zaključava poslije prve uplate. Povrat po pravilima otkazivanja.
- Potpisani dokument je snapshot teksta i verzije šablona i ne mijenja se.
- Neobavezno (ako ostane vremena): oglasna tabla skipera.

## Podjela posla
- **Temelj (zajedno, issue-i T1 do T6):** čišćenje startera, uloge i AgencyId u tokenu, Agency + Notification + kosturi TourSlot/Reservation/Payment, seeder s dvije agencije, interfejsi `IWeatherAdvisory` i `INotificationService`, layout. Radi se prvi, prije razdvajanja na tokove.
- **Flow A:** gost, javna stranica, kalendar, rezervacija, potvrda, forma i ugovor, builder formi, uplate i valute, otkazivanje i povrat, ocjena, prijava agencije i admin panel, spajanje malih grupa.
- **Flow B:** onboarding agencije, skiperi, dionice i ture, čamci i oprema, rasporedi i generisanje termina, kalendar vlasnika, prognoza, raspoređivanje skipera i čamaca, oprema po čamcu, logistika, dashboard skipera, zatvaranje ture i izvještaji, dashboard vlasnika.
- **Ugovor između tokova:** tok A samo čita `TourSlot`, tok B ga puni. Rizik dana A čita samo kroz `IWeatherAdvisory`. `Reservation` i `Payment` su kosturi iz temelja; A puni logiku, B čita. Obavijesti kroz `INotificationService`. Zajedničke entitete (Agency, Notification, MarketUser) ne mijenjaj bez dogovora s kolegom.
- Backlog je u GitHub issue-ima (39 issue-a s pravilima, tabelama i procjenama). Kreni uvijek od issuea koji je korisnik izabrao.

## Pravila za agenta
1. Radi samo na jednom issueu odjednom i na njegovoj grani; nikad direktno na `main`.
2. Prije promjene modela provjeri da li entitet pripada drugom toku; ako da, pitaj korisnika.
3. Svaku validaciju i poslovno pravilo opravdaj pravilom iz ovog dokumenta ili issuea. Ako pravila nema, pitaj, ne izmišljaj.
4. Ne unosi tajne i prave lozinke u kod. Lozinka lokalne baze je samo za razvoj.
5. Poslije promjena pokreni build backenda i frontenda i testove, pa javi stvarni rezultat.
6. Ne mijenjaj `appsettings.json` i zajedničke fajlove osim ako je to zadatak issuea.
7. Odgovaraj na bosanskom, jasno i bez suvišnog teksta.
