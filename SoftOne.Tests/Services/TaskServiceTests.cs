using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SoftOne.Entities;
using SoftOne.Exceptions;
using SoftOne.Mapping;
using SoftOne.Models.Requests;
using SoftOne.Repositories;
using SoftOne.Services;

namespace SoftOne.Tests.Services;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _repositoryMock = new();
    private readonly IMapper _mapper;
    private readonly TaskService _sut;

    public TaskServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        _sut = new TaskService(_repositoryMock.Object, _mapper, NullLogger<TaskService>.Instance);
    }

    [Fact]
    public async Task GetByIdAsync_WhenMissing_ThrowsNotFound()
    {
        _repositoryMock
            .Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskItem?)null);

        var act = async () => await _sut.GetByIdAsync(99);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*99*");
    }

    [Fact]
    public async Task CreateAsync_ReturnsMappedDto()
    {
        var request = new TaskAddRequest
        {
            Title = "Test task",
            Description = "Desc",
            Priority = 2,
            IsCompleted = false
        };

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskItem t, CancellationToken _) =>
            {
                t.Id = 1;
                t.RowVersion = new byte[] { 1, 2, 3, 4 };
                t.CreatedDate = DateTime.UtcNow;
                return t;
            });

        var result = await _sut.CreateAsync(request);

        result.Id.Should().Be(1);
        result.Title.Should().Be("Test task");
        result.Priority.Should().Be(2);
        result.RowVersion.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task DeleteAsync_SoftDeletesExistingTask()
    {
        var entity = new TaskItem
        {
            Id = 5,
            Title = "To delete",
            RowVersion = new byte[] { 1 }
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        await _sut.DeleteAsync(5);

        _repositoryMock.Verify(
            r => r.SoftDeleteAsync(entity, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidRowVersion_ThrowsBusinessException()
    {
        _repositoryMock
            .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TaskItem { Id = 1, Title = "Old", RowVersion = new byte[] { 1 } });

        var request = new TaskUpdateRequest
        {
            Id = 1,
            Title = "New",
            Priority = 1,
            RowVersion = "not-base64!!!"
        };

        var act = async () => await _sut.UpdateAsync(request);

        // AutoMapper may wrap the converter exception
        var exception = await act.Should().ThrowAsync<Exception>();
        var flattened = exception.Which;
        while (flattened is AutoMapperMappingException { InnerException: not null } am)
        {
            flattened = am.InnerException!;
        }

        flattened.Should().BeOfType<BusinessException>()
            .Which.Message.Should().Match("*RowVersion*");
    }

    [Fact]
    public async Task MarkCompletedAsync_UpdatesFlag()
    {
        var entity = new TaskItem
        {
            Id = 3,
            Title = "Task",
            IsCompleted = false,
            RowVersion = new byte[] { 9 }
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(3, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        _repositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _sut.MarkCompletedAsync(3, true);

        result.IsCompleted.Should().BeTrue();
        _repositoryMock.Verify(
            r => r.UpdateAsync(It.Is<TaskItem>(t => t.IsCompleted), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedList()
    {
        _repositoryMock
            .Setup(r => r.GetFilteredAsync(null, null, null, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TaskItem>
            {
                new()
                {
                    Id = 1,
                    Title = "A",
                    Priority = 1,
                    RowVersion = new byte[] { 1 },
                    CreatedDate = DateTime.UtcNow
                },
                new()
                {
                    Id = 2,
                    Title = "B",
                    Priority = 2,
                    RowVersion = new byte[] { 2 },
                    CreatedDate = DateTime.UtcNow
                }
            });

        var result = await _sut.GetAllAsync();

        result.Should().HaveCount(2);
        result.Select(t => t.Title).Should().Contain(new[] { "A", "B" });
    }
}
