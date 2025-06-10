using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.Configuration;
using Server.Data;
using Server.Repository;
using Server.Repository.Interface;
using Server.Service;
using Server.Service.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSetting>(
    builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IRankingRepository, RankingRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRankingService, RankingService>();
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddSingleton<IPasswordService, PasswordService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSetting>();
var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.SecretKey))
    throw new InvalidOperationException("JWT settings are not properly configured.");

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddControllers().
    AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUnity", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowUnity");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();