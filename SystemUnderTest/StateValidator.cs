namespace SystemUnderTest;

public static class StateValidator
{
    public static bool Validate(string code, (string expectedCode, bool isMobile, Uri redirect) knownState)
    {
        if (knownState == default)
            throw new ArgumentNullException(nameof(knownState));

        return code == knownState.expectedCode;
    }
}