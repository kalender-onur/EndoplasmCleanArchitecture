using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using EndoplasmCleanArchitecture.Api.DTOs;
using EndoplasmCleanArchitecture.Api.Middleware;
using EndoplasmCleanArchitecture.Domain.Entities;
using System.Text;
using EndoplasmCleanArchitecture.Presistence;
using EndoplasmCleanArchitecture.Authentication;
using Microsoft.AspNetCore.Identity;
using EndoplasmCleanArchitecture.Presistence.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(p => p.AddPolicy("cors", builder =>
{
    builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();

}));


ConfigurationManager configuration = builder.Configuration;


Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/application.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddLogging(x => x.AddSerilog());

builder.Services.AddApplicationLayer(builder.Configuration);
builder.Services.AddPersistenceLayer(builder.Configuration);
builder.Services.AddAuthenticationLayer(builder.Configuration);

builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JWT").Get<JWT>();
    if (string.IsNullOrEmpty(jwtSettings?.Key))
        throw new ArgumentNullException("JWT Key is not configured.");

    var key = Encoding.ASCII.GetBytes(jwtSettings.Key);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true
    };
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EndoplasmCleanArchitecture Api", Version = "v1" });

    c.MapType<Response<string>>(() => new OpenApiSchema
    {
        Type = "object",
        Properties = new Dictionary<string, OpenApiSchema>
        {
            { "success", new OpenApiSchema { Type = "boolean" } },
            { "message", new OpenApiSchema { Type = "string" } },
            { "data", new OpenApiSchema { Type = "string" } },
            { "statusCode", new OpenApiSchema { Type = "integer" } }
        }
    });
});


var app = builder.Build();



using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await services.GetRequiredService<AppDbContext>().Database.MigrateAsync();

}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("cors");

app.UseMiddleware<ExceptionMiddleware>();

app.UseMiddleware<TokenValidationMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
