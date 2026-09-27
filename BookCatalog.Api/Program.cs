using BookCatalog.Api.Endpoints;
using BookCatalog.Api.Repositories;
using BookCatalog.Api.Services;
using FluentValidation;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Register OpenAPI
builder.Services.AddOpenApi();

// Register Repositories & Services
builder.Services.AddSingleton<IBookRepository, InMemoryBookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

// Automatically scans and registers all FluentValidation validators in the project
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// Map the endpoints
app.MapBookEndpoints();

app.Run();