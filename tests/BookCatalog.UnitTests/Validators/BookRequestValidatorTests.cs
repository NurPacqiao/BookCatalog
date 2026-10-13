using BookCatalog.Api.DTOs;
using BookCatalog.Api.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace BookCatalog.UnitTests.Validators;

public class BookRequestValidatorTests
{
    private readonly CreateBookRequestValidator _createValidator = new();
    private readonly UpdateBookRequestValidator _updateValidator = new();

    // Shared, valid, non-empty AuthorId — the DTOs now carry a Guid, not a string.
    private static readonly Guid AnyAuthorId = Guid.NewGuid();

    [Fact]
    public void CreateValidator_WhenAllFieldsAreValid_ShouldNotHaveErrors()
    {
        var model = new CreateBookRequest(
            Title: "The Hobbit",
            AuthorId: AnyAuthorId,
            Isbn: "978-0261102217",
            PublicationYear: 1937,
            Price: 15.99m);

        var result = _createValidator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreateValidator_WhenTitleIsMissing_ShouldHaveValidationError(string? invalidTitle)
    {
        var model = new CreateBookRequest(
            Title: invalidTitle!,
            AuthorId: AnyAuthorId,
            Isbn: "ISBN",
            PublicationYear: 2020,
            Price: 10m);

        var result = _createValidator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Title)
              .WithErrorMessage("Title is required.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void CreateValidator_WhenPriceIsZeroOrNegative_ShouldHaveValidationError(decimal invalidPrice)
    {
        var model = new CreateBookRequest(
            Title: "Title",
            AuthorId: AnyAuthorId,
            Isbn: "ISBN",
            PublicationYear: 2020,
            Price: invalidPrice);

        var result = _createValidator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Price)
              .WithErrorMessage("Price must be greater than 0.");
    }

    [Fact]
    public void CreateValidator_WhenYearIsInFuture_ShouldHaveValidationError()
    {
        var futureYear = DateTime.UtcNow.Year + 1;
        var model = new CreateBookRequest(
            Title: "Title",
            AuthorId: AnyAuthorId,
            Isbn: "ISBN",
            PublicationYear: futureYear,
            Price: 10m);

        var result = _createValidator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.PublicationYear);
    }

    [Fact]
    public void CreateValidator_WhenAuthorIdIsEmpty_ShouldHaveValidationError()
    {
        var model = new CreateBookRequest(
            Title: "Title",
            AuthorId: Guid.Empty,
            Isbn: "ISBN",
            PublicationYear: 2020,
            Price: 10m);

        var result = _createValidator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.AuthorId)
              .WithErrorMessage("AuthorId is required.");
    }

    [Fact]
    public void UpdateValidator_WhenIsbnIsEmpty_ShouldHaveValidationError()
    {
        var model = new UpdateBookRequest(
            Title: "Title",
            AuthorId: AnyAuthorId,
            Isbn: "",
            PublicationYear: 2020,
            Price: 10m);

        var result = _updateValidator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Isbn);
    }
}