using Microsoft.Extensions.Localization;

public class JsonStringLocalizerFactory : IStringLocalizerFactory
{
    public IStringLocalizer Create(Type resourceSource)
    {
        return new JsonStringLocalizer("en");  // Default to "en" locale
    }

    public IStringLocalizer Create(string baseName, string location)
    {
        return new JsonStringLocalizer(location);  // Use location (language) as parameter
    }
}
