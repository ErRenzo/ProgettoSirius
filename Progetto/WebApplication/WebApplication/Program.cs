using WebApplication;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

var app = builder.Build();
app.UseStaticFiles();
app.UseHttpsRedirection();

WindDataDAL windDAL = new WindDataDAL();
PowerDataDAL powerDAL = new PowerDataDAL();
Turbine serviziTurbina = new Turbine();

// END POINTS
app.MapGet("/", async (HttpContext context) =>
{
    var path = Path.Combine(Directory.GetCurrentDirectory(), "View", "index.html");
    var html = await File.ReadAllTextAsync(path);
    return Results.Content(html, "text/html");

});

app.MapGet("/mAPI", () =>
{
    return Results.Ok("Minimal API Project Work Sirius");
});

app.MapGet("/mAPI/power-data", () =>
{
    List<PowerData> elenco = powerDAL.GetAll();

    return Results.Ok(elenco);
});

app.MapGet("/mAPI/wind-data", () =>
{
    List<WindData> elenco = windDAL.GetAll();

    return Results.Ok(elenco);
});

app.MapGet("/mAPI/combined-data", () =>
{
    List<CombinedData> elenco = serviziTurbina.GetCombined();

    return Results.Ok(elenco);
});
app.MapGet("/mAPI/stats", () =>
{
    var stats = serviziTurbina.GetStats();

    return Results.Ok(stats);
});
app.MapGet("/mAPI/anomalies", () =>
{
    List<AnomalyData> elenco = serviziTurbina.GetAnomalies();

    var risposta = new
    {
        TotaleRilevato = elenco.Count,
        ConteggioSpreco = elenco.Count(x => x.TipoAnomalia == "SPRECO"),
        ConteggioWarning = elenco.Count(x => x.TipoAnomalia == "WARNING"),
        ConteggioCritical = elenco.Count(x => x.TipoAnomalia == "CRITICAL"),
        Dettagli = elenco
    };

    return Results.Ok(risposta);
});
app.MapGet("/mAPI/efficiency", () =>
{
    List<EfficiencyData> report = serviziTurbina.GetEfficiencyAnalysis();
    return Results.Ok(report);
});
app.Run();
