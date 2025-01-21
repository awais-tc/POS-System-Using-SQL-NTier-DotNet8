using POS.Core.Models;
namespace POS.Core.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetProductsAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(ProductDto product);
        Task<ProductDto> UpdateProductAsync(ProductDto product);
        Task DeleteProductAsync(int id);
    }
}