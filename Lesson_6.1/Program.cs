using System.Text;


var task = WriteToFileAsync("../../../test.txt");

Console.WriteLine("some operation");

await task;

async Task WriteToFileAsync(string path)
{
    string message = Console.ReadLine()!;

    await using var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite, 4096,
        FileOptions.Asynchronous);

    byte[] bytes = Encoding.UTF8.GetBytes(message);
    await stream.WriteAsync(bytes, 0, bytes.Length);
    
    Console.WriteLine("after io operation");
}