using XtraWork.API.Requests;
using XtraWork.API.Responses;

namespace XtraWork.API.Services
{
    public interface IEmployeeService
    {
        Task<EmployeeResponse> Create(EmployeeRequest request, CancellationToken cancellationToken);
        Task Delete(Guid id);
        Task<EmployeeResponse> Get(Guid id, CancellationToken cancellationToken);
         Task<List<EmployeeResponse>> GetAll(CancellationToken cancellationToken);
         Task<List<EmployeeResponse>> Search(string keyword);
         Task<EmployeeResponse> Update(Guid id, EmployeeRequest request, CancellationToken cancellationToken);
    }
}