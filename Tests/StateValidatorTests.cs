using FluentAssertions;

namespace Tests;

public class StateValidatorTests
{
    [Theory]
    [AutoData]
    public void WhenCodesMatch_IsValid(string code, bool isMobile, Uri redirect)
        => StateValidator.Validate(code, (code, isMobile, redirect))
            .Should().BeTrue();

    [Theory]
    [AutoData]
    public void WhenCodesDoNotMatch_IsInvalid(string code, string expectedCode, bool isMobile, Uri redirect)
        => StateValidator.Validate(code, (expectedCode, isMobile, redirect))
            .Should().BeFalse();

    [Theory, AutoData]
    public void WhenKnownStateIsDefault_Throws(string code)
        => FluentActions
            .Invoking(() => StateValidator.Validate(code, default))
            .Should().Throw<ArgumentNullException>();
}