using XtraWork.API.Entities;
using XtraWork.API.Repositories;
using XtraWork.API.Requests;
using XtraWork.API.Responses;

namespace XtraWork.API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly Serilog.ILogger _logger;

        public EmployeeService(IEmployeeRepository employeeRepository, Serilog.ILogger logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public async Task<EmployeeResponse> Create(EmployeeRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.FirstName))
            {
                throw new Exception("FirstName cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(request.LastName))
            {
                throw new Exception("LastName cannot be empty.");
            }

            if (request.BirthDate == default)
            {
                throw new Exception("Birthdate cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(request.Gender))
            {
                throw new Exception("Gender cannot be empty.");
            }

            if (request.TitleId == default)
            {
                throw new Exception("TitleId cannot be empty.");
            }

            var employee = new Employee {
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate,
                Gender = request.Gender,
                TitleId = request.TitleId
            };

            await _employeeRepository.Create(employee);

            var response = await Get(employee.Id, cancellationToken);

            return response;
        }

        public async Task Delete(Guid id)
        {
            await _employeeRepository.Delete(id);
        }

        public async Task<List<EmployeeResponse>> GetAll(CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.GetAll(cancellationToken);

            var response = employees.Select(employee => new EmployeeResponse
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                BirthDate = employee.BirthDate,
                Gender = employee.Gender,
                TitleId = employee.TitleId,
                TitleDescription = employee.TitleDescription
            }).ToList();

            return response;
        }

        public async Task<EmployeeResponse> Get(Guid id, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.Get(id, cancellationToken);

            if (employee == null)
            {
                _logger.Information("Unable to find employee with Id: {id}", id);
                return null;
            }

            var response = new EmployeeResponse
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                BirthDate = employee.BirthDate,
                Gender = employee.Gender,
                TitleId = employee.TitleId,
                TitleDescription = employee.TitleDescription
            };

            _logger.Information("Retrieved employee with Id: {id}", id);
            return response;
        }

        public async Task<List<EmployeeResponse>> Search(string keyword)
        {
            var employees = await _employeeRepository.Search(keyword);

            var response = employees.Select(employee => new EmployeeResponse
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                BirthDate = employee.BirthDate,
                Gender = employee.Gender,
                TitleId = employee.TitleId,
                TitleDescription = employee.TitleDescription

            }).ToList();

            return response;
        }

        public async Task<EmployeeResponse> Update(Guid id, EmployeeRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.FirstName))
            {
                throw new Exception("FirstName cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(request.LastName))
            {
                throw new Exception("LastName cannot be empty.");
            }

            if (request.BirthDate == default)
            {
                throw new Exception("Birthdate cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(request.Gender))
            {
                throw new Exception("Gender cannot be empty.");
            }

            if (request.TitleId == default)
            {
                throw new Exception("TitleId cannot be empty.");
            }


            var employee = await _employeeRepository.Get(id, cancellationToken);

            if (employee == null)
                return null;

            employee.FirstName = request.FirstName;
            employee.LastName = request.LastName;
            employee.BirthDate = request.BirthDate;
            employee.Gender = request.Gender;
            employee.TitleId = request.TitleId;

            await _employeeRepository.Update(employee);

            var response = await Get(id, cancellationToken);

            return response;
        }
    }
}