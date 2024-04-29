

var wordsWhichStartFromA = File.ReadAllText("../../../data.txt")
        .Split()                                                                                
        .AsParallel()                                                                                                                                                                                                                   
        .Where(word => word.StartsWith("a") || word.StartsWith("A"));

foreach (var w in wordsWhichStartFromA)
{
        Console.WriteLine(w);
}





