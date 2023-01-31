namespace SystemUnderTest;

public interface IRenderer
{
    string Success((string expectedCode, bool isMobile, Uri redirect) knownState);
    string Failure((string expectedCode, bool isMobile, Uri redirect) knownState);
    string Error((string expectedCode, bool isMobile, Uri redirect) knownState, Exception e);
}

public class Renderer
{
    public string Success((string expectedCode, bool isMobile, Uri redirect) knownState)
    {
        if (knownState.isMobile)
            return "{\"success\": true, \"redirect\": \"" + knownState.redirect + "\"}";
        else
            return "302 Location: " + knownState.redirect;
    }

    public string Failure((string expectedCode, bool isMobile, Uri redirect) knownState)
    {
        if (knownState.isMobile)
            return "{\"success\": false, \"redirect\": \"login\"}";
        else
            return "302 Location: login";
    }

    public string Error((string expectedCode, bool isMobile, Uri redirect) knownState, Exception e)
    {
        if (knownState.isMobile)
            return "{\"error\": \"" + e.Message + "\"}";
        else
            return "500";
    }
}