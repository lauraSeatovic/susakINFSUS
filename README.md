# SUSAK - Sustav za upravljanje atletskim klubom – ASP.NET MVC

Ova aplikacija omogućuje upravljanje atletskim klubom. Razvijena je u sklopu predmeta Informacijski sustavi na Fakultetu elktrotehnike i računarstva z Zagrebu.

## Preduvjeti

- .NET 6 SDK ili noviji  
- SQL Server  
- Visual Studio 2022 ili noviji

## Postavljanje baze

1. **Kreiraj bazu**.
2. **Pokreni SQL skriptu** koja sadrži tablice i početne podatke.
3. **Uredi `appsettings.json`** s odgovarajućim podacima za povezivanje na bazu:

```json
{
  "ConnectionStrings": {
    "susakContext": "Server=localhost;Database=SportskaDb;Trusted_Connection=True;"
  }
}

## Pokretanje aplikacije

1. Otvori `.sln` datoteku u **Visual Studio-u**.

2. U **Solution Exploreru**, odaberi projekt `susak`.

3. Pritisni `ctrl + F5` za pokretanje aplikacije.

4. Aplikacija će se otvoriti u pregledniku.

---

## Pokretanje testova

1. Otvori **Test Explorer** u Visual Studio-u.

2. Klikni na **Run All Tests**..

