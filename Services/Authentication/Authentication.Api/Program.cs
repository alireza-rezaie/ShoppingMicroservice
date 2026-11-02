using System.Text;
using Asp.Versioning;
using Authentication.Application;
using Authentication.Infrustructure;
using Authentication.Infrustructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var jwt = builder.Configuration.GetSection("Jwt");
var signingKey = jwt["Key"] ?? throw new InvalidOperationException("Jwt:Key must be configured.");
if (Encoding.UTF8.GetByteCount(signingKey) < 32)
    throw new InvalidOperationException("Jwt:Key must contain at least 32 bytes.");

builder.Services.AddAuthenticationInfrastructure(builder.Configuration);
builder.Services.AddAuthenticationApplication();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, ValidIssuer = jwt["Issuer"],
        ValidateAudience = true, ValidAudience = jwt["Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
        ValidateLifetime = true, ClockSkew = TimeSpan.FromSeconds(30)
    };
});
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new() { Title = "Authentication.Api", Version = "v1", Description = "Authentication API" }));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    var initialized = false;
    for (var attempt = 1; attempt <= 30 && !initialized; attempt++)
    {
        try
        {
            await db.Database.EnsureCreatedAsync();
            initialized = true;
        }
        catch (Microsoft.Data.SqlClient.SqlException) when (attempt < 30)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }
    app.MapOpenApi();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();