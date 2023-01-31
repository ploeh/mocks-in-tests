using Moq;

namespace Tests;

public class ControllerTests
{
    private readonly RepositoryStub _repository = new RepositoryStub();

    [Theory]
    [AutoData]
    public void HappyPath(string state, (string, bool, Uri) knownState)
    {
        var (expectedCode, _, _) = knownState;
        var code = expectedCode;
        _repository.Add(state, knownState);
        var sut = new Controller(_repository);

        var expected = Renderer.Success(knownState);
        sut
            .Complete(state, code)
            .Should().Be(expected);
    }

    [Theory]
    [AutoData]
    public void Failure(string state, (string, bool, Uri) knownState)
    {
        var (expectedCode, _, _) = knownState;
        var code = expectedCode + "1"; // Any extra string will do
        _repository.Add(state, knownState);
        var sut = new Controller(_repository);

        var expected = new Renderer().Failure(knownState);
        sut
            .Complete(state, code)
            .Should().Be(expected);
    }

    [Theory]
    [AutoData]
    public void Error(string state, string code)
    {
        _repository.Add(state, default);
        var sut = new Controller(_repository);

        sut
            .Complete(state, code)
            .Should().Be("500");
    }
}