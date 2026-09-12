namespace WebApplication
{
    public class Turbine
    {
        private PowerDataDAL powerDAL = new PowerDataDAL();
        private WindDataDAL windDAL = new WindDataDAL();

        // FUNZIONE PER I DATI COMBINATI
        public List<CombinedData> GetCombined()
        {
            List<PowerData> listaPotenza = powerDAL.GetAll();
            List<WindData> listaVento = windDAL.GetAll();

            List<CombinedData> risultato = new List<CombinedData>();

            foreach (var power in listaPotenza)
            {
                CombinedData datoCombinato = new CombinedData();
                datoCombinato.Date = power.Data;
                datoCombinato.Power = power.ActivePower;

                double velocitaTrovata = 0;

                foreach (var wind in listaVento)
                {
                    if (wind.Data == power.Data)
                    {
                        velocitaTrovata = wind.windSpeed;
                        break;
                    }
                }

                datoCombinato.WindSpeed = velocitaTrovata;

                risultato.Add(datoCombinato);
            }

            return risultato;
        }
        // FUNZIONE PER LE STATISTICHE
        public object GetStats()
        {
            var data = GetCombined();

            // Usiamo LINQ qui dentro per calcolare i valori
            var stats = new
            {
                MaxPower = data.Max(x => x.Power),
                AvgPower = data.Average(x => x.Power),
                MaxWind = data.Max(x => x.WindSpeed),
                AvgWind = data.Average(x => x.WindSpeed),
                TotalRecords = data.Count
            };

            return stats;
        }
        // FUNZIONE PER LE ANOMALIE (Allarmi)
        public List<AnomalyData> GetAnomalies()
        {
            var datiCombinati = GetCombined(); // (m/s)
            List<AnomalyData> listaAnomalie = new List<AnomalyData>();

            foreach (var item in datiCombinati)
            {
                // ANOMALIA 1: SPRECO/GUASTO
                // Il vento è nel range giusto (3.5 - 25 m/s) ma la produzione è zero o quasi
                if (item.WindSpeed >= 3.5 && item.WindSpeed <= 25 && item.Power <= 0)
                {
                    listaAnomalie.Add(new AnomalyData
                    {
                        Data = item.Date,
                        Vento = item.WindSpeed,
                        Potenza = item.Power,
                        TipoAnomalia = "SPRECO",
                        Descrizione = "Vento operativo ma produzione assente. Possibile guasto o fermo macchina."
                    });
                }
                // ANOMALIA 2: POTENZA NEGATIVA
                // La turbina sta assorbendo energia dalla rete elettrica invece di produrla
                else if (item.Power < -1)
                {
                    listaAnomalie.Add(new AnomalyData
                    {
                        Data = item.Date,
                        Vento = item.WindSpeed,
                        Potenza = item.Power,
                        TipoAnomalia = "WARNING",
                        Descrizione = "Consumo anomalo: la turbina sta assorbendo energia elettrica."
                    });
                }
                // ANOMALIA 3: MANCATO TAGLIO (CUT-OUT FAIL)
                // Se il vento supera i 25 m/s, la turbina dovrebbe essere ferma (0 kW). 
                // Se produce ancora, c'è un rischio strutturale!
                else if (item.WindSpeed > 25 && item.Power > 10)
                {
                    listaAnomalie.Add(new AnomalyData
                    {
                        Data = item.Date,
                        Vento = item.WindSpeed,
                        Potenza = item.Power,
                        TipoAnomalia = "CRITICAL",
                        Descrizione = "Vento oltre il limite di sicurezza (Cut-out) ma turbina ancora in funzione!"
                    });
                }
            }
            return listaAnomalie;
        }

        // FUNZIONE PER IL CALCOLO DI CALCOLO EFFICIENZA
        public List<EfficiencyData> GetEfficiencyAnalysis()
        {
            var matrice = GetPowerCurveMatrix();
            var datiReali = GetCombined(); // Recupera i dati accoppiati (Vento m/s e Potenza)

            var risultati = new List<EfficiencyData>();

            foreach (var riga in datiReali)
            {
                double vReale = riga.WindSpeed;
                double pTeorica = 0;

                // Calcolo potenza teorica con interpolazione
                if (vReale >= 3.5 && vReale <= 25)
                {
                    PowerCurveData p1 = null;
                    PowerCurveData p2 = null;

                    foreach (var punto in matrice)
                    {
                        if (punto.WindSpeed <= vReale) p1 = punto;
                        if (punto.WindSpeed > vReale) { p2 = punto; break; }
                    }

                    if (p1 != null && p2 != null)
                    {
                        // Formula interpolazione
                        pTeorica = p1.ActivePower + (vReale - p1.WindSpeed) * (p2.ActivePower - p1.ActivePower) / (p2.WindSpeed - p1.WindSpeed);
                    }
                }

                // Calcolo efficienza
                double effPerc = 0;
                if (pTeorica > 0)
                {
                    effPerc = (riga.Power / pTeorica) * 100;
                }

                // Aggiungo il risultato tipizzato
                risultati.Add(new EfficiencyData
                {
                    Data = riga.Date,
                    Vento = Math.Round(vReale, 2),
                    PotenzaReale = Math.Round(riga.Power, 2),
                    PotenzaTeorica = Math.Round(pTeorica, 2),
                    Percentuale = Math.Round(effPerc, 2),
                    EfficienzaTesto = Math.Round(effPerc, 2) + "%"
                });
            }

            return risultati;
        }
        // TABELLA POWER CURVE
        private List<PowerCurveData> GetPowerCurveMatrix()
        {
            return new List<PowerCurveData>
            {
                new PowerCurveData { WindSpeed = 3.5, ActivePower = 0 },
                new PowerCurveData { WindSpeed = 4.0, ActivePower = 77.67 },
                new PowerCurveData { WindSpeed = 4.5, ActivePower = 155.0 },
                new PowerCurveData { WindSpeed = 5.0, ActivePower = 228.33 },
                new PowerCurveData { WindSpeed = 5.5, ActivePower = 318.67 },
                new PowerCurveData { WindSpeed = 6.0, ActivePower = 428.0 },
                new PowerCurveData { WindSpeed = 6.5, ActivePower = 548.33 },
                new PowerCurveData { WindSpeed = 7.0, ActivePower = 688.33 },
                new PowerCurveData { WindSpeed = 7.5, ActivePower = 845.33 },
                new PowerCurveData { WindSpeed = 8.0, ActivePower = 1028.33 },
                new PowerCurveData { WindSpeed = 8.5, ActivePower = 1227.67 },
                new PowerCurveData { WindSpeed = 9.0, ActivePower = 1403.67 },
                new PowerCurveData { WindSpeed = 9.5, ActivePower = 1594.33 },
                new PowerCurveData { WindSpeed = 10.0, ActivePower = 1758.67 },
                new PowerCurveData { WindSpeed = 10.5, ActivePower = 1904.67 },
                new PowerCurveData { WindSpeed = 11.0, ActivePower = 1967.67 },
                new PowerCurveData { WindSpeed = 11.5, ActivePower = 1991.33 },
                new PowerCurveData { WindSpeed = 12.0, ActivePower = 1994.33 },
                new PowerCurveData { WindSpeed = 12.5, ActivePower = 2000.0 },
                new PowerCurveData { WindSpeed = 25.0, ActivePower = 2000.0 }
            };
        }
    }
}
