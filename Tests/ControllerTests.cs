using Moq;
using SystemUnderTest;

namespace Tests;

public class ControllerTests
{
    private readonly RepositoryStub _repository;
    private readonly Controller _sut;

    public ControllerTests()
    {
        _repository = new RepositoryStub();
        _sut = new Controller(_repository);
    }

    [Theory]
    [AutoData]
    public void HappyPath(string state, (string, bool, Uri) knownState)
    {
        var (expectedCode, _, _) = knownState;
        var code = expectedCode;
        _repository.Add(state, knownState);

        var expected = Renderer.Success(knownState);
        _sut
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

        var expected = Renderer.Failure(knownState);
        _sut
            .Complete(state, code)
            .Should().Be(expected);
    }

    [Theory]
    [AutoData]
    public void Error(string state, string code)
    {
        _repository.Add(state, default);

        _sut
            .Complete(state, code)
            .Should().Be("500");
    }
}