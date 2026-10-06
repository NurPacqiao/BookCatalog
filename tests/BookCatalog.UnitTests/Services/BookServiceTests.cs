using BookCatalog.Api.DTOs;
using BookCatalog.Api.Models;
using BookCatalog.Api.Repositories;
using BookCatalog.Api.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace BookCatalog.UnitTests.Services;

public class BookServiceTests
{
    private readonly IBookRepository _repository = Substitute.For<IBookRepository>();
    private readonly ILogger<BookService> _logger = Substitute.For<ILogger<BookService>>();
    private readonly BookService _sut; // SUT = System Under Test

    public BookServiceTests()
    {
        _sut = new BookService(_repository, _logger);
    }

    [Fact]
    public async Task GetAllAsync_WhenCalled_ReturnsPagedResponse()
    {
        var parameters = new BookQueryParameters(Page: 1, PageSize: 10, Search: null, Year: null);
        var books = new List<Book>
        {
            new() { Id = Guid.NewGuid(), Title = "Clean Code", Author = "Robert Martin", Isbn = "123", PublicationYear = 2008, Price = 40m }
        };
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
        var book = new Book
        {
            Id = bookId,
            Title = "Refactoring",
            Author = "Martin Fowler",
            Isbn = "456",
            PublicationYear = 1999,
            Price = 50m
        };
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
        var request = new CreateBookRequest("Clean Architecture", "Robert Martin", "789", 2017, 35m);

        var result = await _sut.CreateAsync(request);

        result.Should().NotBeNull();
        result.Title.Should().Be(request.Title);
        await _repository.Received(1).AddAsync(Arg.Any<Book>());
    }

    [Fact]
    public async Task UpdateAsync_WhenBookDoesNotExist_ReturnsNull()
    {
        var missingId = Guid.NewGuid();
        var request = new UpdateBookRequest("Title", "Author", "ISBN", 2020, 20m);
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