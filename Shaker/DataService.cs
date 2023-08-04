using System.Text.Json;

namespace Shaker; 

internal sealed class DataService {
    public Bar LoadBar(string filepath)
    {
        var barJson = File.ReadAllText(filepath);
        return JsonSerializer.Deserialize<Bar>(barJson) ?? new Bar();
    }

    public void SaveBar(Bar bar, string filepath)
    {
        var barJson = JsonSerializer.Serialize(bar);
        File.WriteAllText(filepath, barJson);
    }
}