using Asp.Versioning;
using Catalog.Application.Commands.Products;
using Catalog.Application.Mapper;
using Catalog.Application.Queries.Brands;
using Catalog.Core.Repositories;
using Catalog.Infrustructure.Data;
using Catalog.Infrustructure.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//Register AutoMapper
builder.Services.AddAutoMapper(
    cfg => { },
    typeof(ProfileMapper)
);
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//Register Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

//Register Mediator
var assemblies = new Assembly[]
{
    Assembly.GetExecutingAssembly(),
    typeof(GetAllProductBrandsQueryHandler).Assembly
};
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));//builder.Services.AddTransient<IRequestHandler<DeleteProductCommand, bool>,DeleteProductCommandHandler>();
//DI
builder.Services.AddScoped<ICatalogContext, CatalogContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ITypeRepository, TypeRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();

builder.Services.AddScoped<IMediator, Mediator>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Catalog.Api", Version = "v1", Description = "Catalog API" });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    //builder.Services.AddSwaggerGen(c =>
    //{
    //    c.SwaggerDoc("v1", new() { Title = "Catalog.Api", Version = "v1", Description = "Catalog API" });
    //});

}
app.UseSwagger();
app.UseSwaggerUI();
//app.MapGet("/", () => Results.Redirect("/swagger"));


app.UseAuthorization();

app.MapControllers();

app.Run();
