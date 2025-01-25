var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseEndpoints(endpoints =>
        {
            // Default route configuration
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Add your board-specific route if needed
            endpoints.MapControllerRoute(
                name: "board",
                pattern: "board/{action=Index}/{id?}",
                defaults: new { controller = "Board", action = "Index" }
            );

            endpoints.MapControllerRoute(
                name: "game",
                pattern: "game/{action=Index}/{id?}",
                defaults: new { controller = "Game", action = "Index" }
            );
        });

app.Run();
