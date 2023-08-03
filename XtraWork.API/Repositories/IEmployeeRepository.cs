using XtraWork.API.Entities;

namespace XtraWork.API.Repositories
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        Task<List<Employee>> Search(string keyword);
    }
}