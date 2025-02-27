﻿namespace SystemUnderTest;

public interface IRepository
{
    (string expectedCode, bool isMobile, Uri redirect) GetState(string login);
}

public class Controller
{
    private readonly IRepository _repository;
    private readonly IStateValidator _stateValidator;
    private readonly IRenderer _renderer;

    public Controller(IRepository repository, IStateValidator stateValidator, IRenderer renderer)
    {
        _repository = repository;
        _stateValidator = stateValidator;
        _renderer = renderer;
    }

    public string Complete(string state, string code)
    {
        var knownState = _repository.GetState(state);
        try
        {
            if (_stateValidator.Validate(code, knownState))
                return _renderer.Success(knownState);
            else
                return _renderer.Failure(knownState);
        }
        catch (Exception e)
        {
            return _renderer.Error(knownState, e);
        }
    }
}