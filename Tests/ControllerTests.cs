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

    [Fact]
    public void Failure()
    {
        (from state in Gen.String
         from code in Gen.String
         from expectedCode in Gen.String.Where(s => s != code)
         from isMobile in Gen.Bool
         let urls = new[] { "https://example.com", "https://example.org" }
         from redirect in Gen.OneOfConst(urls).Select(s => new Uri(s))
         select (state, code, (expectedCode, isMobile, redirect)))
        .Sample((state, code, knownState) =>
        {
            var (expectedCode, _, _) = knownState;
            var repository = new RepositoryStub();
            repository.Add(state, knownState);
            var sut = new Controller(repository);

            var expected = Renderer.Failure(knownState);
            sut
                .Complete(state, code)
                .Should().Be(expected);
        });
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