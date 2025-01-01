using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Linq;

public class JsonStringLocalizer : IStringLocalizer
{
    private readonly string _filePath;

    public JsonStringLocalizer(string language)
    {
        // Set file path to language-specific JSON file
        _filePath = Path.Combine("Resources", $"messages.{language}.json");
    }

    public LocalizedString this[string name]
    {
        get
        {
            var dictionary = LoadLocalization();
            var value = dictionary.ContainsKey(name) ? dictionary[name] : name;
            return new LocalizedString(name, value, resourceNotFound: false);
        }
    }

    public LocalizedString this[string name, params object[] arguments] => throw new NotImplementedException();

    public IEnumerable<LocalizedString> GetAllStrings(bool includeAncestorCultures)
    {
        var dictionary = LoadLocalization();
        return dictionary.Select(kvp => new LocalizedString(kvp.Key, kvp.Value));
    }

    private Dictionary<string, string> LoadLocalization()
    {
        if (!File.Exists(_filePath)) return new Dictionary<string, string>();

        var json = File.ReadAllText(_filePath);
        return JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
    }
}
