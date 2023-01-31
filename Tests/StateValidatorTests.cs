namespace Tests;

public class StateValidatorTests
{
    private readonly StateValidator _target = new();

    [Theory]
    [AutoData]
    public void WhenCodesMatch_IsValid(string code, bool isMobile, Uri redirect)
        => _target
            .Validate(code, (code, isMobile, redirect))
            .Should().BeTrue();

    [Theory]
    [AutoData]
    public void WhenCodesDoNotMatch_IsInvalid(string code, string expectedCode, bool isMobile, Uri redirect)
        => _target
            .Validate(code, (expectedCode, isMobile, redirect))
            .Should().BeFalse();

    [Theory, AutoData]
    public void WhenKnownStateIsDefault_Throws(string code)
        => _target
            .Invoking(target => target.Validate(code, default))
            .Should().Throw<ArgumentNullException>();
}