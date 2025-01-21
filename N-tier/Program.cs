using BusinessLogic.Services;
using DataAccess.Context;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using POS.Core.Interfaces;

var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<POSDbContext>(options =>
                    options.UseSqlServer("Data Source=DESKTOP-KQ5BDOJ\\SQLEXPRESS;Initial Catalog=POSDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;"));

                services.AddScoped<IProductRepository, ProductRepository>();
                services.AddScoped<IProductService, ProductService>();
            })
            .Build();

var productService = host.Services.GetRequiredService<IProductService>();

while (true)
{
    Console.Clear();
    Console.WriteLine("===== POS System Menu =====");
    Console.WriteLine("1. Add Product");
    Console.WriteLine("2. View All Products");
    Console.WriteLine("3. Update Product");
    Console.WriteLine("4. Delete Product");
    Console.WriteLine("5. Exit");
    Console.Write("Enter your choice: ");

    var choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            await AddProduct(productService);
            break;
        case "2":
            await ViewProducts(productService);
            break;
        case "3":
            await UpdateProduct(productService);
            break;
        case "4":
            await DeleteProduct(productService);
            break;
        case "5":
            return;
        default:
            Console.WriteLine("Invalid choice, press Enter to try again...");
            Console.ReadLine();
            break;
    }
}

static async Task AddProduct(IProductService productService)
{
    string name;
    do
    {
        Console.Write("Enter product name: ");
        name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Product name cannot be empty. Please enter a valid name.");
        }
    } while (string.IsNullOrWhiteSpace(name));

    decimal price;
    while (true)
    {
        Console.Write("Enter product price: ");
        if (decimal.TryParse(Console.ReadLine(), out price) && price > 0)
        {
            break;
        }
        else
        {
            Console.WriteLine("Invalid price. Please enter a valid positive decimal value.");
        }
    }

    string description;
    do
    {
        Console.Write("Enter product description: ");
        description = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(description))
        {
            Console.WriteLine("Product description cannot be empty. Please enter a valid description.");
        }
    } while (string.IsNullOrWhiteSpace(description));

    var productDto = new POS.Core.Models.ProductDto
    {
        Name = name,
        Price = price,
        Description = description
    };

    var result = await productService.CreateProductAsync(productDto);
    Console.WriteLine($"Product added successfully! ID: {result.Id}");
    Console.ReadLine();
}


static async Task ViewProducts(IProductService productService)
{
    var products = await productService.GetProductsAsync();
    Console.WriteLine("Product List:");
    foreach (var product in products)
    {
        Console.WriteLine($"{product.Id}: {product.Name} - ${product.Price} - {product.Description}");
    }
    Console.ReadLine();
}

static async Task UpdateProduct(IProductService productService)
{
    Console.Write("Enter product ID to update: ");
    if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
    {
        Console.WriteLine("Invalid product ID. Please enter a valid numeric ID.");
        Console.ReadLine();
        return;
    }

    var existingProduct = await productService.GetProductByIdAsync(id);
    if (existingProduct == null)
    {
        Console.WriteLine("Product not found!");
        Console.ReadLine();
        return;
    }

    string name;
    do
    {
        Console.Write("Enter new product name: ");
        name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Product name cannot be empty. Please enter a valid name.");
        }
    } while (string.IsNullOrWhiteSpace(name));

    decimal price;
    while (true)
    {
        Console.Write("Enter new product price: ");
        if (decimal.TryParse(Console.ReadLine(), out price) && price > 0)
        {
            break;
        }
        else
        {
            Console.WriteLine("Invalid price. Please enter a valid positive decimal value.");
        }
    }

    string description;
    do
    {
        Console.Write("Enter new product description: ");
        description = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(description))
        {
            Console.WriteLine("Product description cannot be empty. Please enter a valid description.");
        }
    } while (string.IsNullOrWhiteSpace(description));

    existingProduct.Name = name;
    existingProduct.Price = price;
    existingProduct.Description = description;

    var updatedProduct = await productService.UpdateProductAsync(existingProduct);
    Console.WriteLine("Product updated successfully!");
    Console.ReadLine();
}


static async Task DeleteProduct(IProductService productService)
{
    Console.Write("Enter product ID to delete: ");
    int id = Convert.ToInt32(Console.ReadLine());

    await productService.DeleteProductAsync(id);
    Console.WriteLine("Product deleted successfully!");
    Console.ReadLine();
}