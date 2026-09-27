using BookCatalog.Api.DTOs;
using BookCatalog.Api.Services;
using FluentValidation;

namespace BookCatalog.Api.Endpoints;

public static class BookEndpoints
{
    public static RouteGroupBuilder MapBookEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/books");

        group.MapGet("/", GetAllBooks);
        group.MapGet("/{id:guid}", GetBookById);
        group.MapPost("/", CreateBook);
        group.MapPut("/{id:guid}", UpdateBook);
        group.MapDelete("/{id:guid}", DeleteBook);

        return group;
    }

    private static async Task<IResult> GetAllBooks(IBookService service)
    {
        var books = await service.GetAllAsync();
        return Results.Ok(books);
    }

    private static async Task<IResult> GetBookById(Guid id, IBookService service)
    {
        var book = await service.GetByIdAsync(id);
        return book is null 
            ? Results.NotFound(new { message = $"Book with ID '{id}' does not exist." }) 
            : Results.Ok(book);
    }

    private static async Task<IResult> CreateBook(
        CreateBookRequest request,
        IValidator<CreateBookRequest> validator,
        IBookService service)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var book = await service.CreateAsync(request);
        return Results.Created($"/books/{book.Id}", book);
    }

    private static async Task<IResult> UpdateBook(
        Guid id,
        UpdateBookRequest request,
        IValidator<UpdateBookRequest> validator,
        IBookService service)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var updatedBook = await service.UpdateAsync(id, request);
        return updatedBook is null 
            ? Results.NotFound(new { message = $"Book with ID '{id}' does not exist." }) 
            : Results.Ok(updatedBook);
    }

    private static async Task<IResult> DeleteBook(Guid id, IBookService service)
    {
        var deleted = await service.DeleteAsync(id);
        return deleted ? Results.NoContent() : Results.NotFound(new { message = $"Book with ID '{id}' does not exist." });
    }
}