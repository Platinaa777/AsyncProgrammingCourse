





var range = Enumerable.Range(1, 10_000_000);

var only2Degree = range
    .AsParallel()
    .Where(x => (x & (x - 1)) == 0)
    .ToList();

foreach (var item in only2Degree)
{
    Console.Write(item + ", ");
}























