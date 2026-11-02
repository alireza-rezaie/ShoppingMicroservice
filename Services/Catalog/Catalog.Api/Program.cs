using System.Reflection;
using System.Text;
using Asp.Versioning;
using Catalog.Application.Mapper;
using Catalog.Application.Queries.Brands;
using Catalog.Core.Repositories;
using Catalog.Infrustructure.Data;
using Catalog.Infrustructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var jwt = builder.Configuration.GetSection("Jwt");
var signingKey = jwt["Key"] ?? throw new InvalidOperationException("Jwt:Key must be configured.");
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
builder.Services.AddAutoMapper(cfg => { }, typeof(ProfileMapper));
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});
var assemblies = new Assembly[] { Assembly.GetExecutingAssembly(), typeof(GetAllProductBrandsQueryHandler).Assembly };
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
builder.Services.AddScoped<ICatalogContext, CatalogContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ITypeRepository, TypeRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new() { Title = "Catalog.Api", Version = "v1", Description = "Catalog API" }));
builder.Services.AddOpenApi();

var app = builder.Build();
if (app.Environment.IsDevelopment()) app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();