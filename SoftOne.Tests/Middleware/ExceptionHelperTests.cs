using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SoftOne.Exceptions;
using SoftOne.Middleware;

namespace SoftOne.Tests.Middleware;

public class ExceptionHelperTests
{
    [Fact]
    public void ResolveDetail_DbUpdateException_InDevelopment_ReturnsRootSqlMessage()
    {
        var ex = new DbUpdateException(
            "An error occurred while saving the entity changes. See the inner exception for details.",
            new InvalidOperationException("Error while saving"));

        var detail = ExceptionHelper.ResolveDetail(ex, isDevelopment: true);

        detail.Should().Be("Invalid column name");
    }

    [Fact]
    public void ResolveDetail_DbUpdateException_InProduction_ReturnsGenericSaveMessage()
    {
        var ex = new DbUpdateException(
            "An error occurred while saving the entity changes. See the inner exception for details.",
            new InvalidOperationException("Error while saving"));

        var detail = ExceptionHelper.ResolveDetail(ex, isDevelopment: false);

        detail.Should().Be("Server Error Please try again later");
    }

    [Fact]
    public void ResolveDetail_UnknownException_InProduction_ReturnsUnexpectedMessage()
    {
        var ex = new InvalidOperationException("Something broke");

        var detail = ExceptionHelper.ResolveDetail(ex, isDevelopment: false);

        detail.Should().Be("Server Error Please try again later");
    }

    [Fact]
    public void Normalize_DbUpdateConcurrency_ReturnsConflictException()
    {
        var normalized = ExceptionHelper.Normalize(new DbUpdateConcurrencyException("conflict"));

        normalized.Should().BeOfType<ConflictException>();
    }
}
