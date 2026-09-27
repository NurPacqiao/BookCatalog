// Validators/CreateBookRequestValidator.cs
using BookCatalog.Api.DTOs;
using FluentValidation;

namespace BookCatalog.Api.Validators;

public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
{
    public CreateBookRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.");

        RuleFor(x => x.Author)
            .NotEmpty().WithMessage("Author is required.");

        RuleFor(x => x.Isbn)
            .NotEmpty().WithMessage("ISBN is required.");

        RuleFor(x => x.PublicationYear)
            .InclusiveBetween(868, DateTime.UtcNow.Year)
            .WithMessage($"Publication year must be between 868 and {DateTime.UtcNow.Year}.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");
    }
}