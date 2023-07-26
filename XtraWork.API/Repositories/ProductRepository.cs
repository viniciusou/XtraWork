using Microsoft.EntityFrameworkCore;
using XtraWork.API.Entities;

namespace XtraWork.API.Repositories
{
    public class ProductRepository
    {
        private readonly XtraWorkContext _context;

        public ProductRepository(XtraWorkContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.Products
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);
        } 
    }
}