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
using Bogus;

namespace Writings.Application.Tests.Unit
{
    public class WritingServiceTests
    {
        private readonly WritingService _sut;
        private readonly IWritingRepository _writingsRepository = Substitute.For<IWritingRepository>();
        private readonly IValidator<Writing> _writingValidator = Substitute.For<IValidator<Writing>>();
        private readonly IValidator<GetAllWritingsOptions> _writingOptionsValidator = Substitute.For<IValidator<GetAllWritingsOptions>>();
        private readonly ILogger<WritingService> _logger = Substitute.For<ILogger<WritingService>>();

        private readonly Faker<Writing> _writingGenerator = new Faker<Writing>()
            .RuleFor(w => w.Id, faker => faker.Random.Guid())
            .RuleFor(w => w.Title, faker => faker.Random.Word())
            .RuleFor(w => w.Body, faker => faker.Random.Words(20))
            .RuleFor(w => w.Type, WritingType.Notes)
            .RuleFor(w => w.YearOfCompletion, faker => faker.Date.Past(1).Year);

        public WritingServiceTests()
        {
            _sut = new WritingService(_writingsRepository, _writingValidator, _writingOptionsValidator, _logger);
        }

        #region CreateAsyncTests

        [Fact(Timeout = 2000)]
        public async Task CreateAsync_ShouldCreateWriting_WhenWritingRequestIsValid()
        {
            //Arrange
            var writing = _writingGenerator.Generate();

            _writingsRepository.CreateAsync(writing).Returns(true);

            //Act
            var result = await _sut.CreateAsync(writing);

            //Assert
            result.Should().Be(true);
        }

        [Theory(Timeout = 2000)]
        [MemberData(nameof(CreateAsync_ShouldNotCreateWriting_WhenWritingRequestIsInvalid_TestData))]
        public async Task CreateAsync_ShouldNotCreateWriting_WhenWritingRequestIsInvalid(Writing writing)
        {
            //Arrange
            _writingsRepository.CreateAsync(writing).Returns(false);

            //Act
            var result = await _sut.CreateAsync(writing);

            //Assert
            result.Should().Be(false);
        }

        [Fact(Timeout = 2000)]
        public async Task CreateAsync_ShouldLogMessageAndException_WhenExceptionIsThrown()
        {
            //Arrange
            var exception = new Exception();

            var writing = _writingGenerator.Generate();

            _writingsRepository.CreateAsync(writing).Throws(exception);

            //Act
            var requestAction = async () => await _sut.CreateAsync(writing);

            //Assert
            await requestAction.Should().ThrowAsync<Exception>();
            _logger.Received(1);
        }

        #endregion

        #region GetAllAsyncTests

        [Fact(Timeout = 2000)]
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

        [Fact(Timeout = 2000)]
        public async Task GetAllAsync_ShouldReturnWritings_WhenSomeWritingsExist()
        {
            //Arrange
            var writing = _writingGenerator.Generate();

            var expectedWritings = new List<Writing>
            {
                writing
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

        [Fact(Timeout = 2000)]
        public async Task GetByIdAsync_ShouldReturnWriting_WhenIdIsValid()
        {
            //Arrange
            var expectedWriting = _writingGenerator.Generate();

            var writingId = expectedWriting.Id;

            _writingsRepository.GetByIdAsync(writingId).Returns(expectedWriting);

            //Act
            var result = await _sut.GetByIdAsync(writingId);

            //Assert
            result.Should().BeEquivalentTo(expectedWriting);
        }

        [Fact(Timeout = 2000)]
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

        [Fact(Timeout = 2000)]
        public async Task UpdateAsync_ShouldLogMessageAndException_WhenExceptionIsThrown()
        {
            //Arrange
            var exception = new Exception();

            var writing = _writingGenerator.Generate();

            _writingsRepository.UpdateAsync(writing).Throws(exception);

            //Act
            var requestAction = async () => await _sut.UpdateAsync(writing);

            //Assert
            await requestAction.Should().ThrowAsync<Exception>();
            _logger.Received(1);
        }

        #endregion

        #region DeleteByIdAsyncTests

        [Fact(Timeout = 2000)]
        public async Task DeleteByIdAsync_ShouldLogMessageAndException_WhenExceptionIsThrown()
        {
            //Arrange
            var exception = new Exception();

            var writingId = Guid.NewGuid();

            _writingsRepository.DeleteByIdAsync(writingId).Throws(exception);

            //Act
            var requestAction = async () => await _sut.DeleteByIdAsync(writingId);

            //Assert
            await requestAction.Should().ThrowAsync<Exception>();
            _logger.Received(1);
        }

        #endregion

        #region GetCountAsyncTests

        [Fact(Timeout = 2000)]
        public async Task GetCountAsync_ShouldReturnZero_WhenNoWritingsFound()
        {
            //Arrange
            _writingsRepository.GetCountAsync(Arg.Any<string?>(), Arg.Any<WritingType?>(), Arg.Any<int?>(), Arg.Any<Guid?>()).Returns(0);

            //Act
            var result = await _sut.GetCountAsync(null, null, null, null);

            //Assert
            result.Should().Be(0);
        }

        [Fact(Timeout = 2000)]
        public async Task GetCountAsync_ShouldReturnTenCount_WhenTenWritingsFound()
        {
            //Arrange
            _writingsRepository.GetCountAsync(Arg.Any<string?>(), Arg.Any<WritingType?>(), Arg.Any<int?>(), Arg.Any<Guid?>()).Returns(10);

            //Act
            var result = await _sut.GetCountAsync(null, null, null, null);

            //Assert
            result.Should().Be(10);
        }

        #endregion

        public static IEnumerable<object[]> CreateAsync_ShouldNotCreateWriting_WhenWritingRequestIsInvalid_TestData =>
            new List<object[]>
            {
                new object[] {
                    new Writing {
                        Id = Guid.NewGuid(),
                        Title = "Title",
                        Body = "Body",
                        Type = WritingType.Notes,
                        YearOfCompletion = DateTimeOffset.Now.AddYears(1).Year
                    }
                },
                new object[] {
                    new Writing {
                        Id = Guid.NewGuid(),
                        Title = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                        Body = "Body",
                        Type = WritingType.Notes,
                        YearOfCompletion = 2023
                    }
                }
            };
    }
}

