Shop shop = new();
for (int i = 0; i < 30; i++)
{
    shop.MakeAnOrder();
}

Action action = () =>
{
    shop.ProcessOrder();
};

var tasks = new List<Task>();

for (int i = 0; i < 10; ++i)
{
    tasks.Add(new Task(action));
}

for (int i = 0; i < 10; i++)
{
    tasks[i].Start();
}

class Shop
{
    private List<Product> _products = new();

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

    public void ProcessOrder()
    {
        var product = _products.FirstOrDefault();

        if (product is not null)
        {
            Console.WriteLine($"Продукт {product.Name} был изъят из коллекции, его количество {product.Quantity}");

            if (!_products.Remove(product))
            {
                Console.WriteLine("Product not found");
            }
        }
    }
}

class Product
{
    public string Name { get; set; }
    public int Quantity { get; set; }
}