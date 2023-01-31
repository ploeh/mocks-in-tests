﻿using Moq;

namespace Tests;

public class ControllerTests
{
    private readonly RepositoryStub _repository = new RepositoryStub();
    private readonly Mock<IStateValidator> _stateValidator = new();
    private readonly Mock<IRenderer> _renderer = new();

    [Theory]
    [AutoData]
    public void HappyPath(string state, (string, bool, Uri) knownState, string response)
    {
        var (expectedCode, _, _) = knownState;
        var code = expectedCode;
        _repository.Add(state, knownState);
        _renderer
            .Setup(renderer => renderer.Success(knownState))
            .Returns(response);
        var sut = new Controller(_repository, new StateValidator(), _renderer.Object);

        sut
            .Complete(state, code)
            .Should().Be(response);
    }

    [Theory]
    [AutoData]
    public void Failure(string state, (string, bool, Uri) knownState, string response)
    {
        var (expectedCode, _, _) = knownState;
        var code = expectedCode + "1"; // Any extra string will do
        _repository.Add(state, knownState);
        _renderer
            .Setup(renderer => renderer.Failure(knownState))
            .Returns(response);
        var sut = new Controller(_repository, new StateValidator(), _renderer.Object);

        sut
            .Complete(state, code)
            .Should().Be(response);
    }

    [Theory]
    [AutoData]
    public void Error(string state, string code, (string, bool, Uri) knownState, Exception e, string response)
    {
        _repository.Add(state, knownState);
        _stateValidator
            .Setup(validator => validator.Validate(code, knownState))
            .Throws(e);
        _renderer
            .Setup(renderer => renderer.Error(knownState, e))
            .Returns(response);
        var sut = new Controller(_repository, _stateValidator.Object, _renderer.Object);

        sut
            .Complete(state, code)
            .Should().Be(response);
    }
}