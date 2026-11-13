using Market.Application.Common;

namespace Market.Tests.AuthTests.UnitTests;

public class PhoneRulesUnitTests
{
    [Theory]
    [InlineData("+38761123456")]
    [InlineData("+387 61 123-456")]
    [InlineData("061/123-456")]
    [InlineData("(061) 123 456")]
    public void IsValid_ValidNumber_ReturnsTrue(string phone)
    {
        Assert.True(PhoneRules.IsValid(phone));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("abcdefgh")]
    [InlineData("38761+123456")]       // "+" only at the start
    [InlineData("+3876112345678901")]  // 16 digits
    public void IsValid_InvalidNumber_ReturnsFalse(string? phone)
    {
        Assert.False(PhoneRules.IsValid(phone));
    }

    [Fact]
    public void Normalize_RemovesSpacesDashesAndBrackets()
    {
        Assert.Equal("+38761123456", PhoneRules.Normalize(" +387 (61) 123-456 "));
    }
}