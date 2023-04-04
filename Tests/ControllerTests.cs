namespace Tests;

using CsCheck;

public class ControllerTests
{
    private static readonly Gen<Uri> _genRedirect = Gen.OneOfConst(new Uri("https://example.com"), new Uri("https://example.org"));

    [Fact]
    public void HappyPath()
    {
         Gen.Select(Gen.String, Gen.String, Gen.Bool, _genRedirect)
        .Sample((state, expectedCode, isMobile, redirect) =>
        {
            var code = expectedCode;
            var knownState = (expectedCode, isMobile, redirect);
            var repository = new RepositoryStub { { state, knownState } };
            var sut = new Controller(repository);
            var actual = sut.Complete(state, code);
            var expected = Renderer.Success(knownState);
            return actual == expected;
        });
    }

    [Fact]
    public void Failure()
    {
        Gen.Select(Gen.String, Gen.String, Gen.String, Gen.Bool, _genRedirect)
        .Where((_, code, expectedCode, _, _) => code != expectedCode)
        .Sample((state, code, expectedCode, isMobile, redirect) =>
        {
            var knownState = (expectedCode, isMobile, redirect);
            var repository = new RepositoryStub { { state, knownState } };
            var sut = new Controller(repository);
            var actual = sut.Complete(state, code);
            var expected = Renderer.Failure(knownState);
            return actual == expected;
        });
    }

    [Fact]
    public void Error()
    {
        Gen.Select(Gen.String, Gen.String)
        .Sample((state, code) =>
        {
            var repository = new RepositoryStub { { state, default } };
            var sut = new Controller(repository);
            var actual = sut.Complete(state, code);
            return actual == "500";
        });
    }
}