using MESDemo.Hubs;
using MESDemo.Services;
using Microsoft.EntityFrameworkCore;
using RadialGraderMES.Data;
using RadialGraderMES.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MESDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("MESConnection")));

builder.Services.AddHttpClient<IPlcService, PlcService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5102/");
    client.Timeout = TimeSpan.FromSeconds(5);
});

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

app.Run();