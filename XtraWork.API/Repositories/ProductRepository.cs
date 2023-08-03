using Microsoft.EntityFrameworkCore;
using XtraWork.API.Entities;

namespace XtraWork.API.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(XtraWorkContext context) : base(context) { }

        public override async Task<List<Product>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.Products
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);
        } 
    }
}