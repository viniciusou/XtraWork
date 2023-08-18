using AutoMapper;
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
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, Serilog.ILogger logger, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _mapper = mapper;
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

            var employee = _mapper.Map<Employee>(request);

            var createdEmployee =  await _employeeRepository.Create(employee);

            var response = _mapper.Map<EmployeeResponse>(createdEmployee);

            return response;
        }

        public async Task Delete(Guid id)
        {
            await _employeeRepository.Delete(id);
        }

        public async Task<List<EmployeeResponse>> GetAll(CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.GetAll(cancellationToken);

            var response = _mapper.Map<List<EmployeeResponse>>(employees);
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

            var response = _mapper.Map<EmployeeResponse>(employee);

            _logger.Information("Retrieved employee with Id: {id}", id);
            return response;
        }

        public async Task<List<EmployeeResponse>> Search(string keyword)
        {
            var employees = await _employeeRepository.Search(keyword);

            var response = _mapper.Map<List<EmployeeResponse>>(employees);

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

            var updatedEmployee = await _employeeRepository.Update(employee);

            var response = _mapper.Map<EmployeeResponse>(updatedEmployee);
            
            return response;
        }
    }
}