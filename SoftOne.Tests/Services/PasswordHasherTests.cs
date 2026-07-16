using FluentAssertions;
using SoftOne.Services;

namespace SoftOne.Tests.Services;

public class PasswordHasherTests
{
    private readonly PasswordHasher _hasher = new();

    [Fact]
    public void Hash_And_Verify_RoundTrip_Succeeds()
    {
        var hash = _hasher.Hash("admin123");

        hash.Should().NotBeNullOrWhiteSpace();
        _hasher.Verify("admin123", hash).Should().BeTrue();
    }

    [Fact]
    public void Verify_WithWrongPassword_ReturnsFalse()
    {
        var hash = _hasher.Hash("admin123");

        _hasher.Verify("wrong-password", hash).Should().BeFalse();
    }

    [Fact]
    public void Hash_ProducesDifferentHashes_ForSamePassword()
    {
        var hash1 = _hasher.Hash("admin123");
        var hash2 = _hasher.Hash("admin123");

        hash1.Should().NotBe(hash2);
        _hasher.Verify("admin123", hash1).Should().BeTrue();
        _hasher.Verify("admin123", hash2).Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("not-a-valid-hash")]
    [InlineData("1.abc")]
    public void Verify_WithInvalidHashFormat_ReturnsFalse(string invalidHash)
    {
        _hasher.Verify("password", invalidHash).Should().BeFalse();
    }
}
