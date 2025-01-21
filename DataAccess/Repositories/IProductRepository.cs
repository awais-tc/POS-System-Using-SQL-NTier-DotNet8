using POS.Core.Models;

namespace DataAccess.Repositories
{
    public interface IProductRepository
    {
        Task<List<ProductDto>> GetProductsAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(ProductDto product);
        Task<ProductDto> UpdateProductAsync(ProductDto product);
        Task DeleteProductAsync(int id);
    }
}
