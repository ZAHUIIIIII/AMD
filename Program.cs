using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=UrlShortener.db")); // Use SQLite database
builder.Services.AddScoped<IUrlService, UrlService>();
builder.Services.AddControllersWithViews(); // Add MVC services

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseStaticFiles(); // Serve static files from wwwroot
app.MapDefaultControllerRoute(); // Use default route for MVC

app.Run();