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
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public override async Task<List<Employee>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.Employees
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Employee>> Search(string keyword)
        {
            return await _context.Employees
                .Where(x => x.FirstName.ToLower().Contains(keyword.ToLower()) || x.LastName.ToLower().Contains(keyword.ToLower()))
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToListAsync();
        }
    }
}