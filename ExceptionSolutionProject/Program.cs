using ExceptionSolutionProject.Data;
using ExceptionSolutionProject.Services;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SignalRChat.Hubs;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHangfire(config =>
config.UseSqlServerStorage(builder.Configuration.GetConnectionString("default")));
builder.Services.AddHangfireServer();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<ApplicationDbContext>(options => options
           .UseSqlServer(builder.Configuration.GetConnectionString("default")));
builder.Services.AddDbContext<ExpHubApplicationDbContext>(options => options
           .UseSqlServer(builder.Configuration.GetConnectionString("ExpHubProject")));
builder.Services.AddScoped<OpenAIService>();
builder.Services.AddHttpClient<SpotifyService>();
builder.Services.AddSignalR();
// SignalR'ı ekleyin
builder.Services.AddSignalR();

// JWT ayarları
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.Use(async (context, next) =>
{
    if (context.Request.Cookies.ContainsKey("AuthToken"))
    {
        var token = context.Request.Cookies["AuthToken"];
        context.Request.Headers["Authorization"] = $"Bearer {token}";
    }
    await next();
});

app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard();
// SignalR hub'ını tanımlayın

// Varsayılan yönlendirmeyi ayarlayın
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=LoginRegister}/{action=Login}/{id?}");

app.Run();
