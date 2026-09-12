var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.MapGet("/", () => Results.Redirect("/swagger"));


app.UseAuthorization();

app.MapControllers();

app.Run();
