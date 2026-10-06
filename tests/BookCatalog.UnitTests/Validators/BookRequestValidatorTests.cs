using BookCatalog.Api.DTOs;
using BookCatalog.Api.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace BookCatalog.UnitTests.Validators;

public class BookRequestValidatorTests
{
    private readonly CreateBookRequestValidator _createValidator = new();
    private readonly UpdateBookRequestValidator _updateValidator = new();

    [Fact]
    public void CreateValidator_WhenAllFieldsAreValid_ShouldNotHaveErrors()
    {
        var model = new CreateBookRequest("The Hobbit", "J.R.R. Tolkien", "978-0261102217", 1937, 15.99m);

        var result = _createValidator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreateValidator_WhenTitleIsMissing_ShouldHaveValidationError(string? invalidTitle)
    {
        var model = new CreateBookRequest(invalidTitle!, "Author", "ISBN", 2020, 10m);

        var result = _createValidator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Title)
              .WithErrorMessage("Title is required.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void CreateValidator_WhenPriceIsZeroOrNegative_ShouldHaveValidationError(decimal invalidPrice)
    {
        var model = new CreateBookRequest("Title", "Author", "ISBN", 2020, invalidPrice);

        var result = _createValidator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Price)
              .WithErrorMessage("Price must be greater than 0.");
    }

    [Fact]
    public void CreateValidator_WhenYearIsInFuture_ShouldHaveValidationError()
    {
        var futureYear = DateTime.UtcNow.Year + 1;
        var model = new CreateBookRequest("Title", "Author", "ISBN", futureYear, 10m);

        var result = _createValidator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.PublicationYear);
    }

    [Fact]
    public void UpdateValidator_WhenIsbnIsEmpty_ShouldHaveValidationError()
    {
        var model = new UpdateBookRequest("Title", "Author", "", 2020, 10m);

        var result = _updateValidator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Isbn);
    }
}