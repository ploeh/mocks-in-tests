namespace Tests;

public class RendererTests
{
    private readonly Renderer _renderer = new();

    [Theory]
    [AutoData]
    public void Success_Regular(string expectedCode, Uri redirect)
        => Renderer
            .Success((expectedCode, false, redirect))
            .Should().Be("302 Location: " + redirect);

    [Theory]
    [AutoData]
    public void Success_Mobile(string expectedCode, Uri redirect)
        => Renderer
            .Success((expectedCode, true, redirect))
            .Should().Be("{\"success\": true, \"redirect\": \"" + redirect + "\"}");

    [Theory]
    [AutoData]
    public void Failure_Regular(string expectedCode, Uri redirect)
        => _renderer
            .Failure((expectedCode, false, redirect))
            .Should().Be("302 Location: login");

    [Theory]
    [AutoData]
    public void Failure_Mobile(string expectedCode, Uri redirect)
        => _renderer
            .Failure((expectedCode, true, redirect))
            .Should().Be("{\"success\": false, \"redirect\": \"login\"}");

    [Theory]
    [AutoData]
    public void Error_Regular(string expectedCode, Uri redirect, Exception e)
        => _renderer
            .Error((expectedCode, false, redirect), e)
            .Should().Be("500");

    [Theory]
    [AutoData]
    public void Error_Mobile(string expectedCode, Uri redirect, Exception e)
        => _renderer
            .Error((expectedCode, true, redirect), e)
            .Should().Be("{\"error\": \"" + e.Message + "\"}");
}