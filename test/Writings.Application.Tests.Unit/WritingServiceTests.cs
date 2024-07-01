using Writings.Application.Models;
using Writings.Application.Services;
using Writings.Application.Enums;
using Xunit;
using FluentAssertions;
using Writings.Application.Repositories.Interfaces;
using NSubstitute;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute.ExceptionExtensions;

namespace Writings.Application.Tests.Unit
{
    public class WritingServiceTests : IDisposable
    {
        private readonly WritingService _sut;
        private readonly IWritingRepository _writingsRepository = Substitute.For<IWritingRepository>();
        private readonly IValidator<Writing> _writingValidator = Substitute.For<IValidator<Writing>>();
        private readonly IValidator<GetAllWritingsOptions> _writingOptionsValidator = Substitute.For<IValidator<GetAllWritingsOptions>>();
        private readonly ILogger<WritingService> _logger = Substitute.For<ILogger<WritingService>>();

        public WritingServiceTests()
        {
            _sut = new WritingService(_writingsRepository, _writingValidator, _writingOptionsValidator, _logger);
        }

        #region CreateAsyncTests

        [Fact]
        public async Task CreateAsync_ShouldCreateWriting_WhenWritingRequestIsValid()
        {
            //Arrange
            var writing = new Writing
            {
                Id = Guid.NewGuid(),
                Title = "Title",
                Body = "Body",
                Type = WritingType.Notes,
                YearOfCompletion = 2023
            };

            _writingsRepository.CreateAsync(writing).Returns(true);

            //Act
            var result = await _sut.CreateAsync(writing);

            //Assert
            result.Should().Be(true);
        }

        [Fact]
        public async Task CreateAsync_ShouldNotCreateWriting_WhenWritingRequestYearOfCompletionIsInvalid()
        {
            //Arrange
            var writing = new Writing
            {
                Id = Guid.NewGuid(),
                Title = "Title",
                Body = "Body",
                Type = WritingType.Notes,
                YearOfCompletion = DateTimeOffset.Now.AddYears(1).Year
            };

            _writingsRepository.CreateAsync(writing).Returns(false);

            //Act
            var result = await _sut.CreateAsync(writing);

            //Assert
            result.Should().Be(false);
        }

        [Fact]
        public async Task CreateAsync_ShouldNotCreateWriting_WhenWritingRequestTitleIsInvalid()
        {
            //Arrange
            var writing = new Writing
            {
                Id = Guid.NewGuid(),
                Title = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                Body = "Body",
                Type = WritingType.Notes,
                YearOfCompletion = 2023
            };

            _writingsRepository.CreateAsync(writing).Returns(false);

            //Act
            var result = await _sut.CreateAsync(writing);

            //Assert
            result.Should().Be(false);
        }

        #endregion

        #region GetAllAsyncTests

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoWritingsExist()
        {
            //Arrange
            _writingsRepository.GetAllAsync(Arg.Any<GetAllWritingsOptions>()).Returns(Enumerable.Empty<Writing>());

            var writingsOptions = new GetAllWritingsOptions
            {
                Title = null,
                Type = null,
                YearOfCompletion = null,
                TagId = null,
                SortField = null,
                SortOrder = null,
                Page = 1,
                PageSize = 10,
                UserId = null
            };

            //Act
            var result = await _sut.GetAllAsync(writingsOptions);

            //Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnWritings_WhenSomeWritingsExist()
        {
            //Arrange
            var testWriting = new Writing
            {
                Id = Guid.NewGuid(),
                Title = "Title",
                Body = "Body",
                Type = WritingType.Notes,
                YearOfCompletion = 2023
            };

            var expectedWritings = new List<Writing>
            {
                testWriting
            };

            _writingsRepository.GetAllAsync(Arg.Any<GetAllWritingsOptions>()).Returns(expectedWritings);

            var writingsOptions = new GetAllWritingsOptions
            {
                Title = null,
                Type = null,
                YearOfCompletion = null,
                TagId = null,
                SortField = null,
                SortOrder = null,
                Page = 1,
                PageSize = 10,
                UserId = null
            };

            //Act
            var result = await _sut.GetAllAsync(writingsOptions);

            //Assert
            result.Should().BeEquivalentTo(expectedWritings);
        }

        #endregion

        #region GetByIdAsyncTests

        [Fact]
        public async Task GetByIdAsync_ShouldReturnWriting_WhenIdIsValid()
        {
            //Arrange
            var writingId = Guid.NewGuid();

            var expectedWriting = new Writing
            {
                Id = writingId,
                Title = "Title",
                Body = "Body",
                Type = WritingType.Notes,
                YearOfCompletion = 2023
            };

            _writingsRepository.GetByIdAsync(writingId).Returns(expectedWriting);

            //Act
            var result = await _sut.GetByIdAsync(writingId);

            //Assert
            result.Should().BeEquivalentTo(expectedWriting);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldNotReturnWriting_WhenIdIsInvalid()
        {
            //Arrange
            var writingId = Guid.NewGuid();

            _writingsRepository.GetByIdAsync(writingId).Returns(null as Writing);

            //Act
            var result = await _sut.GetByIdAsync(writingId);

            //Assert
            result.Should().BeEquivalentTo(null as Writing);
        }

        #endregion

        #region UpdateAsyncTests

        [Fact]
        public async Task UpdateAsync_ShouldLogMessageAndException_WhenExceptionIsThrown()
        {
            //Arrange
            var exception = new Exception();

            var writing = new Writing
            {
                Id = Guid.NewGuid(),
                Title = "Title",
                Body = "Body",
                Type = WritingType.Notes,
                YearOfCompletion = 2023
            };

            _writingsRepository.UpdateAsync(writing).Throws(exception);

            //Act
            var requestAction = async () => await _sut.UpdateAsync(writing);

            //Assert
            await requestAction.Should().ThrowAsync<Exception>();
            _logger.Received(1);
        }

        #endregion

        public void Dispose()
        {
            
        }
    }
}
