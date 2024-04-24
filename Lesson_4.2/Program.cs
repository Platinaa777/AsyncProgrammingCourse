




var test = File.ReadAllText("../../../input.txt");

var task = ParseAsync(test);

Console.WriteLine("Введите свое имя: ");
var name = Console.ReadLine();

await using FileStream fileStream = new FileStream("../../../out.txt", FileMode.Append);

// Создание экземпляра класса StreamWriter на основе FileStream
await using StreamWriter streamWriter = new StreamWriter(fileStream);

var parseResult = await task;

streamWriter.Write($"{name} нашел {parseResult.Count} уникальных слов.\n");

for (int i = 0; i < parseResult.Count - 1; ++i)
    streamWriter.Write(parseResult[i] + ", ");

streamWriter.Write(parseResult[^1] + ".");

async Task<IList<string>> ParseAsync(string inputData)
{
    var words = await Task.Run(() => inputData.Split(new[] { ' ', ',', '.' }).Distinct().ToList());

    return words;
}






