using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using SRMAgreement.Controllers;
using SRMAgreement.Data_Base;
using SRMAgreement.Logger;
using SRMAgreement.SuppCode;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataBaseContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging(false); // Опционально: отключение логирования чувствительных данных
    options.UseLoggerFactory(LoggerFactory.Create(builder => builder
        .AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning) // Уровень логирования для команд EF Core
        .AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning))); // Уровень логирования для остальных логов EF Core
});

builder.Services.AddDbContext<DataBaseArchive>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("ArchiveConnection"));
    options.UseLoggerFactory(LoggerFactory.Create(builder => builder
        .AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning)
        .AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning)));
});

builder.Services.AddDbContext<DataBaseUser>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("UserConnection"));
    options.UseLoggerFactory(LoggerFactory.Create(builder => builder
        .AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning)
        .AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning)));
});

builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "Avers";
        options.LoginPath = "/Login";
        options.ExpireTimeSpan = TimeSpan.FromDays(10);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();
builder.Services.AddSingleton<DataBase>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var env = sp.GetRequiredService<IWebHostEnvironment>();
    return new DataBase(configuration, env);
});

builder.Services.Configure<FormOptions>(options =>
{
    options.ValueCountLimit = int.MaxValue;
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = long.MaxValue;
});

builder.Services.AddHostedService<DatabaseBackupService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var username = "Anonymous";
    if (context.User.Identity.IsAuthenticated)
    {
        var claim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName);
        if (claim != null)
        {
            username = claim.Value;
        }
    }

    app.Logger.LogInformation($"Path: {context.Request.Path}  Time: {DateTime.Now.ToLongTimeString()}  User: {username} Method:{context.Request.Method} Status Code:{context.Response.StatusCode}\n");
    await next.Invoke();
});

app.MapHub<WordHub>("/wordHub");

app.MapRazorPages();
app.MapControllers();

app.Run();
