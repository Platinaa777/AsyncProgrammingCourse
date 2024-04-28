using System.Collections.Concurrent;

Shop shop = new();
for (int i = 0; i < 30; i++)
{
    shop.MakeAnOrder();
}

Func<bool> action = () => shop.ProcessOrder();

var tasks = new List<Task>();

for (int i = 0; i < 10; ++i)
{
    tasks.Add(new Task(() =>
    {
        while (action.Invoke())
        {
            Thread.Sleep(500);
        }
    }));
}

for (int i = 0; i < 10; i++)
{
    tasks[i].Start();
}

Task.WaitAll(tasks.ToArray());

class Shop
{
    private readonly BlockingCollection<Product> _products = new();

    public void MakeAnOrder()
    {
        var rnd = new Random();

        Product product = new Product()
        {
            Name = $"Product - {rnd.Next(0, 100000)}",
            Quantity = rnd.Next(1, 100)
        };
        
        _products.Add(product);
    }

    public bool ProcessOrder()
    {
        if (_products.TryTake(out var product))
        {
            Console.WriteLine($"Продукт {product.Name} был изъят из коллекции, его количество {product.Quantity}");
            return true;
        }

        return false;
    }
}

class Product
{
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
}