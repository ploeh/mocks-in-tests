namespace SystemUnderTest;

public interface IRepository
{
    (string expectedCode, bool isMobile, Uri redirect) GetState(string login);
}

public class Controller
{
    private readonly IRepository _repository;

    public Controller(IRepository repository)
    {
        _repository = repository;
    }

    public string Complete(string state, string code)
    {
        var knownState = _repository.GetState(state);
        try
        {
            if (StateValidator.Validate(code, knownState))
                return Renderer.Success(knownState);
            else
                return Renderer.Failure(knownState);
        }
        catch (Exception e)
        {
            return Renderer.Error(knownState, e);
        }
    }
}