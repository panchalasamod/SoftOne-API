using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoftOne.Controllers;
using SoftOne.Models.Dtos;
using SoftOne.Models.Requests;
using SoftOne.Services;

namespace SoftOne.Tests.Controllers;

public class TasksControllerTests
{
    private readonly Mock<ITaskService> _taskService = new();
    private readonly TasksController _sut;

    public TasksControllerTests()
    {
        _sut = new TasksController(_taskService.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithTasks()
    {
        var tasks = new List<TaskDto>
        {
            new() { Id = 1, Title = "One", Priority = 1 },
            new() { Id = 2, Title = "Two", Priority = 2 }
        };

        _taskService
            .Setup(s => s.GetAllAsync(null, null, null, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tasks);

        var result = await _sut.GetAll(null, null, null);

        var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(tasks);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction()
    {
        var request = new TaskAddRequest { Title = "New", Priority = 1 };
        var created = new TaskDto { Id = 10, Title = "New", Priority = 1 };

        _taskService
            .Setup(s => s.CreateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(created);

        var result = await _sut.Create(request, CancellationToken.None);

        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.Value.Should().BeEquivalentTo(created);
        createdResult.ActionName.Should().Be(nameof(TasksController.GetById));
    }

    [Fact]
    public async Task Update_WhenIdsMismatch_ReturnsBadRequest()
    {
        var request = new TaskUpdateRequest
        {
            Id = 2,
            Title = "X",
            Priority = 1,
            RowVersion = "AAAA"
        };

        var result = await _sut.Update(1, request, CancellationToken.None);

        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Delete_ReturnsNoContent()
    {
        _taskService
            .Setup(s => s.DeleteAsync(1, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _sut.Delete(1, CancellationToken.None);

        result.Should().BeOfType<NoContentResult>();
    }
}
