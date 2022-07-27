var builder = WebApplication.CreateBuilder(args);

// Add services to the container (equivalent to ConfigureServices method in Startup.cs)
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddSignalR();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" }));

// Configure the HTTP request pipeline (equivalent to Configure method in Startup.cs)
var app = builder.Build();
// Exception handling middleware always comes at the top of the middleware container
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
// CORS middleware must be called right after UseRouting; ordering is crucial
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("https://localhost:4200"));
app.UseAuthentication();
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");
app.MapFallbackToController("Index", "Fallback");

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(userManager, roleManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

await app.RunAsync();
