# 🌀 Wind Turbine Stats - Backend Service
> **Project Work Sirius** | Classe 5°DI AS 2025-2026  
> **Sito Impianto:** Coordinate `[41.03001111, 15.59496667]`

Il sistema di Backend per il monitoraggio e l'analisi delle performance di una turbina eolica. Il servizio gestisce l'importazione di dataset storici tramite file CSV, popolando il database per l'analisi dell'efficienza energetica e la rilevazione di anomalie.

---

## 🛠️ Stack Tecnologico
Il servizio è sviluppato con architettura **REST** (Minimal API) per garantire velocità e leggerezza:

* **Framework:** ASP.NET Core 8
* **Linguaggio:** C#
* **Architettura:** Minimal API per un'esposizione dei dati veloce e senza sovrastrutture
* **Database:** SQL Server Management Studio 19
* **Classi ADO.NET:** Insieme di classi che espongono servizi di accesso ai dati. Ponte tra codice C# e qualsiasi database (SQL Server).
* **LINQ:** Per la programmazione dichiarativa in C# (tipo SQL)
* **Data Source:** Importazione tramite file CSV.

## 📋 Funzionalità Principali (Obbligatorio)

Il sistema si concentra sull'elaborazione dei dati locali per visualizzare il comportamento della turbina:

*   **⚙️ Gestione Dati CSV:** Implementazione di un parser (DAL) per file di produzione e meteorologici.
*   **🗄️ Persistence locale:** Archiviazione dei dati importati su tabelle SQL dedicate per garantire la consultazione rapida tramite le API.
*   **📊 Analisi e Correlazione:** Logica di aggregazione (Join) a livello codice (❌ LINQ ❌ SQL) tra della della turbina e la velocità del vento.
*   **⚠️ Analisi Qualitativa:** Confronto tra i dataset per identificare giornate con anomalie (es. alta ventosità ma produzione insufficiente).

## 📋 Analisi efficienza (Opzionale)

Il backend integra un modulo per calcolare la potenza teorica giornaliera mediante **interpolazione lineare** basata sulla power curve della turbina.

### Formula di Interpolazione
Dati due punti della power curve $(v_1, P_1)$ e $(v_2, P_2)$, per un valore di vento $v$ non presente nel dataset:

$$P(v) = P_1 + \frac{(v - v_1)(P_2 - P_1)}{v_2 - v_1}$$

### Calcolo Efficienza
Viene calcolato lo scostamento percentuale per monitorare il rendimento dell'impianto:

$$\text{Efficienza} = \frac{\text{Potenza Reale}}{\text{Potenza Teorica}} \times 100\%$$

---

## 🔌 API Endpoints

Documentazione dei punti di accesso per il Minimal API Project Work Sirius. Tutti gli endpoint restituiscono dati in formato JSON.

| Metodo | Endpoint | Descrizione | Stato |
| :--- | :--- | :--- | :---: |
| `GET` | `/` | Restituisce un messaggio di conferma Index (Ospiterà l'Index del sito) | ✅ |
| `GET` | `/mAPI` | Restituisce un messaggio (Per comprovare la funzionalità del API) | ✅ |
| `GET` | `/mAPI/power-data` | Recupera l'elenco completo dei dati di potenza della turbina | ✅ |
| `GET` | `/mAPI/wind-data` | Recupera l'elenco completo dei dati storici del vento | ✅ |
| `GET` | `/mAPI/combined-data` | Restituisce i dati combinati (Vento + Potenza) della turbina | ✅ |
| `GET` | `/mAPI/stats` | Fornisce le statistiche generali elaborate dai servizi turbina | ✅ |
| `GET` | `/mAPI/anomalies` | Analisi delle anomalie (Spreco, Warning, Critical) con conteggi | ✅ |
| `GET` | `/mAPI/efficiency` | Genera un report sull'analisi dell'efficienza energetica | ✅ |

---

## 📂 Struttura del Database (SchemaLogico)

Il database `DBTurbinStats` è organizzato per supportare query ad alte prestazioni:

*   **Tabella `Power_hourly`**: Contiene `power_id`, `Data`, `Power`.
*   **Tabella `wind_cleaned`**: Contiene `wind_id`, `Data`, `Windspeed`.

---

## 🚀 Istruzioni per l'avvio dell'applicazione Web
1. **Requisiti:**
   
    *  Avere SQL Server
    *  Avere Visual Studio 2022 (O versioni moderne)

3. **Installare l'applicazione**

4. **Inserire il DB:**

    Se il Database non è presente sul server (Da oggetti SQL Server) allora crearlo tramite la seguente query:
   
    ```
    EXEC sp_attach_db @dbname = N'DBTurbinStats',    
    @filename1 = N'C:\...\Progetto\DB\DBTurbinStats.mdf',    
    @filename2 = N'C:\...\Progetto\DB\DBTurbinStats_log.ldf'; 
    ```

4.  **Configurazione Database:**

    Modificare la stringa di connessione nei file DAL di Power_hourly e wind_cleaned con i tuoi parametri locali:
    * *Visual Studio* -> Visualizza -> Esplora oggetti di SQLServer -> SQLServer -> (localdb) -> Propietà -> Stringa di connessione

        ```
        "Server=YOUR_SERVER;Database=WindTurbineDB;Trusted_Connection=True;..."
        ```

5.  **Avvio Applicazione:**

    Avvia il progetto tramite Visual Studio o CLI:
    
        ```
        Bash
        dotnet run
        ```

---

## 👥 Team Backend

| Sviluppatore | Ruolo | GitHub |
| :--- | :--- | :--- |
| Aguayo Renzo | Sviluppatore Backend | [@littlezeggio](https://github.com/littlezeggio) |
| Zeggio Gabriel | Sviluppatore Backend | [@ErRenzo](https://github.com/ErRenzo) |
