namespace SystemUnderTest;

public interface IStateValidator
{
    bool Validate(string code, (string expectedCode, bool isMobile, Uri redirect) knownState);
}

public class StateValidator : IStateValidator
{
    public bool Validate(string code, (string expectedCode, bool isMobile, Uri redirect) knownState)
    {
        if (knownState == default)
            throw new ArgumentNullException(nameof(knownState));

        return code == knownState.expectedCode;
    }
}