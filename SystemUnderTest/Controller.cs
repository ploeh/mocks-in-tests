namespace SystemUnderTest;

public interface IRepository
{
    (string expectedCode, bool isMobile, Uri redirect) GetState(string login);
}

public class Controller
{
    private readonly IRepository _repository;
    private readonly StateValidator _stateValidator;
    private readonly Renderer _renderer;

    public Controller(IRepository repository)
    {
        _repository = repository;
        _stateValidator = new StateValidator();
        _renderer = new Renderer();
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