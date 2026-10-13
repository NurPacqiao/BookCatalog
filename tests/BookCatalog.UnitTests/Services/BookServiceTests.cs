using BookCatalog.Api.DTOs;
using BookCatalog.Api.Services;
using BookCatalog.Domain.Common;
using BookCatalog.Domain.Entities;
using BookCatalog.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace BookCatalog.UnitTests.Services;

public class BookServiceTests
{
    private readonly IBookRepository _repository = Substitute.For<IBookRepository>();
    private readonly ILogger<BookService> _logger = Substitute.For<ILogger<BookService>>();
    private readonly BookService _sut;

    public BookServiceTests()
    {
        _sut = new BookService(_repository, _logger);
    }

    private static Book CreateBookWithAuthor(
        Guid? id = null,
        string title = "Clean Code",
        string authorName = "Robert Martin",
        string isbn = "123",
        int year = 2008,
        decimal price = 40m)
    {
        var author = new Author { Name = authorName };
        return new Book
        {
            Id = id ?? Guid.NewGuid(),
            Title = title,
            Author = author,
            AuthorId = author.Id,
            Isbn = isbn,
            PublicationYear = year,
            Price = price
        };
    }

    [Fact]
    public async Task GetAllAsync_WhenCalled_ReturnsPagedResponse()
    {
        var parameters = new BookQueryParameters(Page: 1, PageSize: 10, Search: null, Year: null);
        var books = new List<Book> { CreateBookWithAuthor() };
        _repository.GetAllAsync(parameters).Returns((books, 1));

        var result = await _sut.GetAllAsync(parameters);

        result.Should().NotBeNull();
        result.TotalCount.Should().Be(1);
        result.Items.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetByIdAsync_WhenBookExists_ReturnsBookResponse()
    {
        var bookId = Guid.NewGuid();
        var book = CreateBookWithAuthor(
            id: bookId,
            title: "Refactoring",
            authorName: "Martin Fowler",
            isbn: "456",
            year: 1999,
            price: 50m);
        _repository.GetByIdAsync(bookId).Returns(book);

        var result = await _sut.GetByIdAsync(bookId);

        result.Should().NotBeNull();
        result!.Id.Should().Be(bookId);
    }

    [Fact]
    public async Task GetByIdAsync_WhenBookDoesNotExist_ReturnsNull()
    {
        var missingId = Guid.NewGuid();
        _repository.GetByIdAsync(missingId).Returns((Book?)null);

        var result = await _sut.GetByIdAsync(missingId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ValidRequest_CallsRepositoryAdd()
    {
        var request = new CreateBookRequest(
            Title: "Clean Architecture",
            AuthorId: Guid.NewGuid(),
            Isbn: "789",
            PublicationYear: 2017,
            Price: 35m);

        var createdBook = new Book
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Isbn = request.Isbn,
            PublicationYear = request.PublicationYear,
            Price = request.Price,
            AuthorId = request.AuthorId,
            Author = new Author { Id = request.AuthorId, Name = "Robert Martin" }
        };
        _repository.GetByIdAsync(Arg.Any<Guid>()).Returns(createdBook);

        var result = await _sut.CreateAsync(request);

        result.Should().NotBeNull();
        result.Title.Should().Be(request.Title);
        await _repository.Received(1).AddAsync(Arg.Any<Book>());
    }

    [Fact]
    public async Task UpdateAsync_WhenBookDoesNotExist_ReturnsNull()
    {
        var missingId = Guid.NewGuid();
        var request = new UpdateBookRequest(
            Title: "Title",
            AuthorId: Guid.NewGuid(),
            Isbn: "ISBN",
            PublicationYear: 2020,
            Price: 20m);
        _repository.GetByIdAsync(missingId).Returns((Book?)null);

        var result = await _sut.UpdateAsync(missingId, request);

        result.Should().BeNull();
        await _repository.DidNotReceive().UpdateAsync(Arg.Any<Book>());
    }

    [Fact]
    public async Task DeleteAsync_WhenRepositoryReturnsTrue_ReturnsTrue()
    {
        var bookId = Guid.NewGuid();
        _repository.DeleteAsync(bookId).Returns(true);

        var result = await _sut.DeleteAsync(bookId);

        result.Should().BeTrue();
    }
}