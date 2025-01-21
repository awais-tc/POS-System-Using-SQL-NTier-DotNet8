using DataAccess.Context;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using POS.Core.Interfaces;
using BusinessLogic.Services;
using Microsoft.Graph.Models.Security;

class Program
{
    static async Task Main(string[] args)
    {
        var host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
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
    }

    static async Task AddProduct(IProductService productService)
    {
        Console.Write("Enter product name: ");
        var name = Console.ReadLine();

        Console.Write("Enter product price: ");
        decimal price = Convert.ToDecimal(Console.ReadLine());

        Console.Write("Enter product description: ");
        var description = Console.ReadLine();

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
        int id = Convert.ToInt32(Console.ReadLine());

        var existingProduct = await productService.GetProductByIdAsync(id);
        if (existingProduct == null)
        {
            Console.WriteLine("Product not found!");
            Console.ReadLine();
            return;
        }

        Console.Write("Enter new product name: ");
        existingProduct.Name = Console.ReadLine();

        Console.Write("Enter new product price: ");
        existingProduct.Price = Convert.ToDecimal(Console.ReadLine());

        Console.Write("Enter new product description: ");
        existingProduct.Description = Console.ReadLine();

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

}
