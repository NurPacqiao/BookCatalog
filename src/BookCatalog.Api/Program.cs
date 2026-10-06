using BookCatalog.Api.Endpoints;
using BookCatalog.Infrastructure.Repositories;
using BookCatalog.Domain.Interfaces;
using BookCatalog.Api.Services;
using FluentValidation;
using Scalar.AspNetCore;
using BookCatalog.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddSingleton<IBookRepository, InMemoryBookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapBookEndpoints();

app.Run();