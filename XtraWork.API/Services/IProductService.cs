using XtraWork.API.Entities;

namespace XtraWork.API.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAll(CancellationToken cancellationToken);    
    }
}