﻿using Moq;

namespace Tests;

public class ControllerTests
{
    private readonly RepositoryStub _repository = new RepositoryStub();
    private readonly Mock<IStateValidator> _stateValidator = new();
    private readonly Mock<IRenderer> _renderer = new();

    private readonly Controller _target;

    public ControllerTests()
    {
        _target = new Controller(_repository, _stateValidator.Object, _renderer.Object);
    }

    [Theory]
    [AutoData]
    public void HappyPath(string state, string code, (string, bool, Uri) knownState, string response)
    {
        _repository.Add(state, knownState);
        _stateValidator
            .Setup(validator => validator.Validate(code, knownState))
            .Returns(true);
        _renderer
            .Setup(renderer => renderer.Success(knownState))
            .Returns(response);

        _target
            .Complete(state, code)
            .Should().Be(response);
    }

    [Theory]
    [AutoData]
    public void Failure(string state, string code, (string, bool, Uri) knownState, string response)
    {
        _repository.Add(state, knownState);
        _stateValidator
            .Setup(validator => validator.Validate(code, knownState))
            .Returns(false);
        _renderer
            .Setup(renderer => renderer.Failure(knownState))
            .Returns(response);

        _target
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

        _target
            .Complete(state, code)
            .Should().Be(response);
    }
}