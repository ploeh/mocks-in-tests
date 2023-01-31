using CsCheck;

namespace Tests;

public class ControllerTests
{
    [Theory]
    [AutoData]
    public void HappyPath(string state, (string, bool, Uri) knownState)
    {
        var (expectedCode, _, _) = knownState;
        var code = expectedCode;
        var repository = new RepositoryStub();
        repository.Add(state, knownState);
        var sut = new Controller(repository);

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
        var repository = new RepositoryStub();
        repository.Add(state, knownState);
        var sut = new Controller(repository);

        var expected = Renderer.Failure(knownState);
        sut
            .Complete(state, code)
            .Should().Be(expected);
    }

    [Fact]
    public void Error()
    {
        (from state in Gen.String
         from code in Gen.String
         select (state, code))
        .Sample((state, code) =>
        {
            var repository = new RepositoryStub();
            repository.Add(state, default);
            var sut = new Controller(repository);

            sut
                .Complete(state, code)
                .Should().Be("500");
        });
    }
}