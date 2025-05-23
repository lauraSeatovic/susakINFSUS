using Microsoft.EntityFrameworkCore;
using susak.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure environment-specific JSON configuration files
IConfiguration configuration = builder.Environment.IsDevelopment()
    ? builder.Configuration.AddJsonFile("appsettings.Development.json").Build()
    : builder.Configuration.AddJsonFile("appsettings.json").Build();

// Configure the DbContext
builder.Services.AddDbContext<susakContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("susak"))
);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IDisciplinaRepository, DisciplinaRepository>();
builder.Services.AddScoped<IDisciplinaService, DisciplinaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
