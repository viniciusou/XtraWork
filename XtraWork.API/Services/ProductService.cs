using Microsoft.Extensions.Caching.Memory;
using XtraWork.API.Entities;
using XtraWork.API.Repositories;

namespace XtraWork.API.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly IMemoryCache _memoryCache;

        public ProductService(ProductRepository productRepository, IMemoryCache memoryCache)
        {
            _productRepository = productRepository;
            _memoryCache = memoryCache;
        }

        public Task<List<Product>> GetAll(CancellationToken cancellationToken)
        {
            return _productRepository.GetAll(cancellationToken);
        }
    }
}