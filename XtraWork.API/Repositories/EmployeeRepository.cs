using Microsoft.EntityFrameworkCore;
using XtraWork.API.Entities;

namespace XtraWork.API.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(XtraWorkContext context) : base(context) { }

        public override async Task<Employee> Get(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Employees
                .Include(x => x.Title)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public override async Task<List<Employee>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.Employees
                .Include(x => x.Title)
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Employee>> Search(string keyword)
        {
            return await _context.Employees
                .Include(x => x.Title)
                .Where(x => x.FirstName.Contains(keyword) || x.LastName.Contains(keyword))
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToListAsync();
        }
    }
}