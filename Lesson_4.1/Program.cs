

var task = CalculateFactorialAsync(12);

while (!task.IsCompleted)
{
    Console.Write('#');
}

int CalculateFactorial(int x)
{
    int multiply = 1;
    while (x > 0)
    {
        multiply *= x;
        x -= 1;
    }

    return multiply;
}

async Task CalculateFactorialAsync(int x)
{
    var result = await Task.Run(() => CalculateFactorial(x));

    Console.WriteLine(result);
}










