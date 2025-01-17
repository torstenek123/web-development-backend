/*
 * !!!!! OBS !!!!!!
 * This project uses the local webapi so that has to be running in the background
 * Open terminal in AppGoodFriendsWebApi folder run:
 * dotnet run -lp https 
 *
 *
 *
 */















using Services;
using DbContext;
using Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region adding support for several secret sources and database sources
//to use either user secrets or azure key vault depending on UseAzureKeyVault tag in appsettings.json
builder.Configuration.AddApplicationSecrets("../Configuration/Configuration.csproj");

//use multiple Database connections and their respective DbContexts
builder.Services.AddDatabaseConnections(builder.Configuration);
builder.Services.AddDatabaseConnectionsDbContext();
#endregion

#region dependency injection

builder.Services.AddScoped<IFriendsService, FriendsServiceWapi>();
builder.Services.AddHttpClient(name: "FriendsWebApi", configureClient: options =>
{
    options.BaseAddress = new Uri("https://localhost:7066/api/");
    options.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(
            mediaType: "application/json",
            quality: 1.0));
});
#endregion 

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

//Map aliases
app.MapGet("/Overview", () => Results.Redirect("/Overview/OverviewCountry"));


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
