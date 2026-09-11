using Npgsql;
using MESDemo.Hubs;
using MESDemo.Services;
using Microsoft.EntityFrameworkCore;
using RadialGraderMES.Data;
using RadialGraderMES.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("MESConnection");

Console.WriteLine(
    $"MES CONNECTION HOST: {connectionString?.Split(';')
        .FirstOrDefault(x => x.StartsWith("Host=", StringComparison.OrdinalIgnoreCase))}"
);

if (!string.IsNullOrWhiteSpace(connectionString))
{
    var csb = new Npgsql.NpgsqlConnectionStringBuilder(connectionString);

    Console.WriteLine(
        $"MES DB: Host={csb.Host}, " +
        $"Port={csb.Port}, " +
        $"Database={csb.Database}, " +
        $"Username={csb.Username}"
    );
}
else
{
    Console.WriteLine("MES DB CONNECTION STRING IS NULL");
}

builder.Services.AddDbContext<MESDbContext>(options =>
    options.UseNpgsql(connectionString));

// PLC Simulator
builder.Services.AddSingleton<PlcSimulationService>();

// PLC Service
builder.Services.AddSingleton<IPlcService, PlcService>();

// PLC Polling
builder.Services.AddHostedService<PlcPollingService>();

builder.Services.AddSignalR();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapHub<MesHub>("/mesHub");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MESDbContext>();
    db.Database.Migrate();
}

app.Run();