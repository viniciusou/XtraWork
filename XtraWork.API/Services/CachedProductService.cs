using Microsoft.Extensions.Caching.Memory;
using XtraWork.API.Entities;
using XtraWork.API.Repositories;

namespace XtraWork.API.Services
{
    public class CachedProductService : IProductService
    {
        private const string ProductListCacheKey = "ProductList";
        private readonly IProductService _productService;
        private readonly IMemoryCache _memoryCache;
        
        public CachedProductService(IProductService productService, IMemoryCache memoryCache)
        {
            _productService = productService;
            _memoryCache = memoryCache;
        }

        public async Task<List<Product>> GetAll(CancellationToken cancellationToken)
        {
            var options = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(60));

            if (_memoryCache.TryGetValue(ProductListCacheKey, out List<Product> result))
                return result;

            result = await _productService.GetAll(cancellationToken);

            _memoryCache.Set(ProductListCacheKey, result, options);

            return result;
        }
    }
}