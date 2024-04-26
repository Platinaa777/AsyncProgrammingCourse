

await LoadPageAsync("https://learn.microsoft.com/ru-ru/dotnet/csharp/tour-of-csharp/");

async Task LoadPageAsync(string url)
{
    var httpClient = new HttpClient();

    var content = await httpClient.GetStringAsync(url);

    Console.WriteLine(content);

    Console.WriteLine("Count microsoft words: " + await CountMicrosoftWordAsync(content));
}

async Task<int> CountMicrosoftWordAsync(string content)
{
    var result = await Task.Run(() =>
    {
        var counter = content.Split(new []{"Microsoft", "microsoft"}, StringSplitOptions.TrimEntries);

        return counter.Length - 1;
    });

    return result;
}


