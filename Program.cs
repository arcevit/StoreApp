using Microsoft.AspNetCore.HttpLogging;
using Scalar.AspNetCore;
using StoreApp.Context;
using StoreApp.Extensions;
using StoreApp.Products;
using StoreApp.Ingredients;
using StoreApp.Profiles;


var builder = WebApplication.CreateBuilder(args);

// Get port from environment variable or use default
var port = Environment.GetEnvironmentVariable("APP_PORT") ?? "8080";

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Servers = [new() { Url = $"http://localhost:{port}" }];
        return Task.CompletedTask;
    });
});


builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>();
});



builder.Services.AddControllers();

// Configure the database
var connectionString = builder.Configuration.GetConnectionString("Store");
builder.Services.AddSqlServer<StoreDbContext>(connectionString);

// Register services
builder.Services.AddScoped<IProductService, ProductsService>();
builder.Services.AddScoped<IIngredientService, IngredientsService>();

builder.Services.AddHttpLogging(o =>
{
    if (builder.Environment.IsDevelopment())
    {
        o.CombineLogs = true;
        o.LoggingFields = HttpLoggingFields.ResponseBody | HttpLoggingFields.ResponseHeaders;
    }
});

var app = builder.Build();

// Migrate the database and seed data
if (app.Environment.IsDevelopment())
{
    await app.MigrateDatabaseAsync();
    await app.SeedDatabaseAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.HttpClient);
    });
}

app.UseForwardedHeaders();

app.Map("/", () => Results.Redirect("/scalar/v1"));


app.MapControllers();

app.Run();
