using System.Text;
using Lumicore.Domain.core.ioc;
using Lumicore.Endpoint.auth;
using Lumicore.Endpoint.middleware;
using Lumicore.Infra;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

Locator.Load(new ProdInjector());

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load(".env");

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddSingleton<IJwtTokenFactory, JwtTokenFactory>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        {
            var jwt = builder.Configuration.GetSection("Jwt");
            var key = jwt["Key"] ?? throw new InvalidOperationException("Jwt:Key manquante");
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = !string.IsNullOrWhiteSpace(jwt["Issuer"]),
                ValidIssuer = jwt["Issuer"],
                ValidateAudience = !string.IsNullOrWhiteSpace(jwt["Audience"]),
                ValidAudience = jwt["Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateLifetime = true
            };
        }
    });


var app = builder.Build();

app.UseCors(policyBuilder =>
{
    policyBuilder.AllowAnyHeader();
    policyBuilder.AllowAnyMethod();
    policyBuilder.AllowAnyOrigin();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<Lumicore.Endpoint.middleware.ConnectedUserMiddleware>();

app.MapControllers();

app.Run();