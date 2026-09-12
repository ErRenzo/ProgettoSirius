# PROJECT_PLAN.md — Wind Turbine Stats
**Classe 5° | Progetto Scolastico**
**Coordinate impianto:** 41.03001111, 15.59496667

---

## 1. Stack Tecnologico

### Backend
| Componente | Tecnologia |
|---|---|
| Framework | ASP.NET Core 8 |
| Linguaggio | C# |
| ORM | Entity Framework Core |
| Database | SQL Server Developer Edition |
| HTTP Client | HttpClient / open-meteo-dotnet-client-sdk |
| API Style | REST (controller-based) |

### Frontend
| Componente | Tecnologia |
|---|---|
| Framework | Nuxt 4 |
| UI Library | PrimeVue |
| Grafici | Chart.js / PrimeVue Charts |
| Linguaggio | TypeScript / JavaScript |

### Servizi Esterni
| Servizio | Utilizzo |
|---|---|
| Open-Meteo API | Dati meteo in tempo reale (velocità vento) |

### Architettura
![Architettura](/Immagini/architettura1.png)

---

## 2. MVP (Minimum Viable Product)

L'MVP rappresenta la versione minima funzionante del progetto, corrispondente al **Task 1 obbligatorio**.

### Funzionalità incluse nell'MVP

1. **Import CSV turbina**
   - Caricamento del file CSV con i dati di active power (kW) e timestamp Unix
   - Parsing e salvataggio su tabella `TurbineData` nel database

2. **Recupero dati meteo**
   - Chiamata all'API Open-Meteo per le coordinate dell'impianto
   - Calcolo velocità media giornaliera del vento
   - Salvataggio su tabella `WeatherData` collegata tramite data alla tabella turbina

3. **Grafico principale**
   - Visualizzazione della active power reale nel tempo (line chart)
   - Overlay della velocità del vento nello stesso periodo
   - Sincronizzazione temporale tra i due dataset

4. **Analisi qualitativa base**
   - Sezione commenti/note sulle anomalie rilevate
   - Identificazione visiva di giorni con vento alto ma bassa produzione

### Fuori dall'MVP (Task 2 – opzionale)
- Calcolo efficienza tramite interpolazione lineare della power curve
- Grafico efficienza giornaliera/settimanale
- Analisi degrado prestazioni nel tempo

---

## 3. WBS (Work Breakdown Structure)

```
Wind Turbine Stats
│
├── 1. Pianificazione
│   ├── 1.1 Definizione requisiti
│   ├── 1.2 Suddivisione ruoli (2 Backend / 2 Frontend)
│   └── 1.3 Stesura PROJECT_PLAN.md
│
├── 2. Backend (ASP.NET Core)
│   ├── 2.1 Setup progetto e struttura soluzione
│   ├── 2.2 Configurazione SQL Server e Entity Framework
│   ├── 2.3 Creazione modelli e migration DB
│   │   ├── 2.3.1 Tabella TurbineData
│   │   └── 2.3.2 Tabella WeatherData
│   ├── 2.4 Import CSV
│   │   ├── 2.4.1 Parsing file CSV
│   │   └── 2.4.2 Endpoint API per upload
│   ├── 2.5 Integrazione Open-Meteo
│   │   ├── 2.5.1 Chiamata API meteo per coordinate impianto
│   │   └── 2.5.2 Salvataggio dati giornalieri su DB
│   ├── 2.6 Endpoint REST per il Frontend
│   │   ├── 2.6.1 GET dati turbina per range date
│   │   └── 2.6.2 GET dati meteo per range date
│   └── 2.7 (Opzionale) Endpoint calcolo efficienza
│
├── 3. Frontend (Nuxt 4 + PrimeVue)
│   ├── 3.1 Setup progetto Nuxt 4
│   ├── 3.2 Configurazione PrimeVue e Chart.js
│   ├── 3.3 Pagina principale – Dashboard
│   │   ├── 3.3.1 Componente grafico Power vs Vento
│   │   ├── 3.3.2 Filtro per range date
│   │   └── 3.3.3 Sezione analisi qualitativa
│   ├── 3.4 Pagina import CSV
│   └── 3.5 (Opzionale) Pagina efficienza
│
├── 4. Integrazione & Test
│   ├── 4.1 Test comunicazione Frontend ↔ Backend
│   ├── 4.2 Test import CSV e visualizzazione dati
│   ├── 4.3 Test dati meteo e sincronizzazione
│   └── 4.4 Verifica grafico e analisi
│
└── 5. Documentazione & Consegna
    ├── 5.1 Completamento PROJECT_PLAN.md
    ├── 5.2 README con istruzioni di avvio
    └── 5.3 Presentazione finale
```

---

## 4. GANTT

> Durata stimata: **8 settimane** (76 giorni lavorativi, contando le task opzionali)

> Team: 2 Backend (BE1, BE2) + 2 Frontend (FE1, FE2)

![GANNT](/Immagini/gannt.png)

**Legenda:**
- `█` = giorno di lavoro attivo su quel task
- **S1–S8** = Settimana 1–8
- **L M M G V** = Lunedì → Venerdì
- **BE1/BE2** = sviluppatori Backend
    - **BE1**: Aguayo Renzo
    - **B22**: Zeggio Gabriel 
- **FE1/FE2** = sviluppatori Frontend
    - **BE1**: Ricca Alessandro
    - **B22**: Corsaro Filippo
- **(Opt.)** = Task opzionale (Background verde)

---