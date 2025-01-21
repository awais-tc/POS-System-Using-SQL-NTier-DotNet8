using POS.Core.Interfaces;
using POS.Core.Models;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductDto>> GetProductsAsync() =>
            await _productRepository.GetProductsAsync();

        public async Task<ProductDto> GetProductByIdAsync(int id) =>
            await _productRepository.GetProductByIdAsync(id);

        public async Task<ProductDto> CreateProductAsync(ProductDto product) =>
            await _productRepository.CreateProductAsync(product);

        public async Task<ProductDto> UpdateProductAsync(ProductDto product) =>
            await _productRepository.UpdateProductAsync(product);

        public async Task DeleteProductAsync(int id) =>
            await _productRepository.DeleteProductAsync(id);
    }
}
