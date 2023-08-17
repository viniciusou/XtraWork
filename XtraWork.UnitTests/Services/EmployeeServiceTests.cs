using NSubstitute;
using NSubstitute.ReturnsExtensions;
using XtraWork.API.Entities;
using XtraWork.API.Repositories;
using XtraWork.API.Requests;
using XtraWork.API.Services;

namespace XtraWork.UnitTests.Services
{
    public class EmployeeServiceTests
    {
        private readonly IEmployeeRepository _repository = Substitute.For<IEmployeeRepository>();
        private readonly Serilog.ILogger _logger = Substitute.For<Serilog.ILogger>();
        private readonly EmployeeService _service;

        public EmployeeServiceTests()
        {
            _service = new EmployeeService(_repository, _logger);
        }

        [Fact]
        public async Task Create_ShouldReturnException_WhenFirstNameIsNullOrEmpty()
        {
            //Arrange
            var request = new EmployeeRequest
            {
                FirstName = null
            };
            var exceptionMessage = "FirstName cannot be empty.";

            //Act
            var response = _service.Create(request, CancellationToken.None);

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => response);
            Assert.Equal(exceptionMessage, exception.Message);
        }

        [Fact]
        public async Task Create_ShouldReturnException_WhenLastNameIsNullOrEmpty()
        {
            //Arrange
            var request = new EmployeeRequest
            {
                FirstName = "John",
                LastName = string.Empty
            };
            var exceptionMessage = "LastName cannot be empty.";

            //Act
            var response = _service.Create(request, CancellationToken.None);

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => response);
            Assert.Equal(exceptionMessage, exception.Message);
        }

        [Fact]
        public async Task Create_ShouldReturnException_WhenBirthdateIsEmpty()
        {
            //Arrange
            var request = new EmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe"
            };
            var exceptionMessage = "Birthdate cannot be empty.";

            //Act
            var response = _service.Create(request, CancellationToken.None);

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => response);
            Assert.Equal(exceptionMessage, exception.Message);
        }

        [Fact]
        public async Task Create_ShouldReturnException_WhenGenderIsNullOrEmpty()
        {
            //Arrange
            var request = new EmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = DateTime.Today.AddYears(-18)
            };
            var exceptionMessage = "Gender cannot be empty.";

            //Act
            var response = _service.Create(request, CancellationToken.None);

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => response);
            Assert.Equal(exceptionMessage, exception.Message);
        }

        [Fact]
        public async Task Create_ShouldReturnException_WhenTitleIdIsNullOrEmpty()
        {
            //Arrange
            var request = new EmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = DateTime.Today.AddYears(-18),
                Gender = "Male"
            };
            var exceptionMessage = "TitleId cannot be empty.";

            //Act
            var response = _service.Create(request, CancellationToken.None);

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => response);
            Assert.Equal(exceptionMessage, exception.Message);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedEmployee_WhenRequestDataIsOk()
        {
            //Arrange
            var request = new EmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = DateTime.Today.AddYears(-18),
                Gender = "Male",
                TitleId = Guid.NewGuid()
            };
            var title = new Title
            {
                Id = request.TitleId,
                Description = "description"
            };
            var employee = new Employee {
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate,
                Gender = request.Gender,
                TitleId = request.TitleId,
                TitleDescription = title.Description
            };

            _repository.Get(employee.Id, Arg.Any<CancellationToken>()).Returns(employee);
            _repository.Create(employee).Returns(employee);

            //Act
            var response = await _service.Create(request, CancellationToken.None);

            //Assert
            Assert.Equal(request.FirstName, response.FirstName);
            Assert.Equal(request.LastName, response.LastName);
            Assert.Equal(request.BirthDate, response.BirthDate);
            Assert.Equal(request.Gender, response.Gender);
            Assert.Equal(request.TitleId, response.TitleId);
        }

        [Fact]
        public async Task Delete_ShouldRemoveEmployee_WhenMethodIsCalled()
        {
            //Arrange
            var employeeId = Guid.NewGuid();

            //Act
            await _service.Delete(employeeId);
            
            //Assert
            await _repository.Received(1).Delete(employeeId);            
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmptyList_WhenThereAreNoEmployees()
        {
            //Arrange
            var cancellationToken = CancellationToken.None;
            _repository.GetAll(Arg.Any<CancellationToken>()).Returns(new List<Employee>());
            
            //Act
            var response = await _service.GetAll(cancellationToken);
            
            //Assert
            Assert.Empty(response);
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmployeeList_WhenThereAreEmployees()
        {
            //Arrange
            var cancellationToken = CancellationToken.None;
            var firstEmployeeId = Guid.NewGuid();
            var secondEmployeeId = Guid.NewGuid();
            var title = new Title
            {
                Id = Guid.NewGuid(),
                Description = "description"
            };
            var employeeList = new List<Employee> 
            {
                new Employee 
                {
                    Id = firstEmployeeId,
                    FirstName = "John",
                    LastName = "Doe",
                    BirthDate = DateTime.Today.AddYears(-18),
                    Gender = "Male",
                    TitleId = title.Id,
                    TitleDescription = title.Description
                },
                new Employee
                {
                    Id = secondEmployeeId,
                    FirstName = "Mary",
                    LastName = "Jane",
                    BirthDate = DateTime.Today.AddYears(-18),
                    Gender = "Female",
                    TitleId = title.Id,
                    TitleDescription = title.Description
                }
            };
            _repository.GetAll(Arg.Any<CancellationToken>()).Returns(employeeList);
            
            //Act
            var response = await _service.GetAll(cancellationToken);
            
            //Assert
            Assert.NotEmpty(response);
            Assert.Equal(2, response.Count);
            Assert.Equal(firstEmployeeId, response[0].Id);
            Assert.Equal(secondEmployeeId, response[1].Id);
        }

        [Fact]
        public async Task Get_ShouldReturnNothing_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var employeeId = Arg.Any<Guid>();
            var cancellationToken = Arg.Any<CancellationToken>();
            _repository.Get(employeeId, cancellationToken).ReturnsNull();

            // Act
            var response = await _service.Get(Guid.NewGuid(), cancellationToken);

            // Assert
            Assert.Null(response);
        }

        [Fact]
        public async Task Get_ShouldLogUnableToFindEmployeeMessage_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var anyEmployeeId = Arg.Any<Guid>();
            var searchedEmployeeId = Guid.NewGuid();
            var cancellationToken = Arg.Any<CancellationToken>();
            _repository.Get(anyEmployeeId, cancellationToken).ReturnsNull();

            // Act
            var response = await _service.Get(searchedEmployeeId, cancellationToken);

            // Assert
            _logger.Received(1).Information("Unable to find employee with Id: {id}", searchedEmployeeId);
        }

        [Fact]
        public async Task Get_ShouldReturnEmployee_WhenEmployeeExists()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var employeeFirstName = "John";
            var cancellationToken = CancellationToken.None;
            var title = new Title
            {
                Id = Guid.NewGuid(),
                Description = "description"
            };
            var employee = new Employee
            {
                Id = employeeId,
                FirstName = employeeFirstName,
                LastName = "Doe",
                BirthDate = DateTime.Today.AddYears(-18),
                Gender = "Male",
                TitleId = title.Id,
                TitleDescription = title.Description
            };

            _repository.Get(employeeId, cancellationToken).Returns(employee);

            // Act
            var response = await _service.Get(employeeId, cancellationToken);

            // Assert
            Assert.Equal(employeeId, response.Id);
            Assert.Equal(employeeFirstName, response.FirstName);
        }

        [Fact]
        public async Task Get_ShouldLogEmployeeRetrievedMessage_WhenEmployeeExists()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var employeeFirstName = "John";
            var cancellationToken = CancellationToken.None;
            var title = new Title
            {
                Id = Guid.NewGuid(),
                Description = "description"
            };
            var employee = new Employee
            {
                Id = employeeId,
                FirstName = employeeFirstName,
                LastName = "Doe",
                BirthDate = DateTime.Today.AddYears(-18),
                Gender = "Male",
                TitleId = title.Id,
                TitleDescription = title.Description
            };

            _repository.Get(employeeId, cancellationToken).Returns(employee);

            // Act
            var response = await _service.Get(employeeId, cancellationToken);

            // Assert
            _logger.Received(1).Information("Retrieved employee with Id: {id}", employeeId);
        }

        [Fact]
        public async Task Search_ShouldReturnEmptyList_WhenThereAreNoEmployeesContainingKeyword()
        {
            //Arrange
            var keyword = "jo";
            _repository.Search(Arg.Any<string>()).Returns(new List<Employee>());
            
            //Act
            var response = await _service.Search(keyword);
            
            //Assert
            Assert.Empty(response);
        }

        [Fact]
        public async Task Search_ShouldReturnEmployeeList_WhenThereAreEmployeesContainingKeyword()
        {
            //Arrange
            var keyword = "john";
            var firstEmployeeId = Guid.NewGuid();
            var title = new Title
            {
                Id = Guid.NewGuid(),
                Description = "description"
            };
            var employeeList = new List<Employee> 
            {
                new Employee 
                {
                    Id = firstEmployeeId,
                    FirstName = "John",
                    LastName = "Doe",
                    BirthDate = DateTime.Today.AddYears(-18),
                    Gender = "Male",
                    TitleId = title.Id,
                    TitleDescription = title.Description
                }
            };
            _repository.Search(keyword).Returns(employeeList);
            
            //Act
            var response = await _service.Search(keyword);
            
            //Assert
            Assert.Single(response);
            Assert.Equal(firstEmployeeId, response[0].Id);
            Assert.Contains(keyword.ToLower(), response[0].FirstName.ToLower());
        }

        [Fact]
        public async Task Update_ShouldReturnException_WhenFirstNameIsNullOrEmpty()
        {
            //Arrange
            var employeeId = Guid.NewGuid();
            var request = new EmployeeRequest
            {
                FirstName = null
            };
            var exceptionMessage = "FirstName cannot be empty.";

            //Act
            var response = _service.Update(employeeId, request, CancellationToken.None);

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => response);
            Assert.Equal(exceptionMessage, exception.Message);
        }

        [Fact]
        public async Task Update_ShouldReturnException_WhenLastNameIsNullOrEmpty()
        {
            //Arrange
            var employeeId = Guid.NewGuid();
            var request = new EmployeeRequest
            {
                FirstName = "John",
                LastName = string.Empty
            };
            var exceptionMessage = "LastName cannot be empty.";

            //Act
            var response = _service.Update(employeeId, request, CancellationToken.None);

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => response);
            Assert.Equal(exceptionMessage, exception.Message);
        }

        [Fact]
        public async Task Update_ShouldReturnException_WhenBirthdateIsEmpty()
        {
            //Arrange
            var employeeId = Guid.NewGuid();
            var request = new EmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe"
            };
            var exceptionMessage = "Birthdate cannot be empty.";

            //Act
            var response = _service.Update(employeeId, request, CancellationToken.None);

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => response);
            Assert.Equal(exceptionMessage, exception.Message);
        }

        [Fact]
        public async Task Update_ShouldReturnException_WhenGenderIsNullOrEmpty()
        {
            //Arrange
            var employeeId = Guid.NewGuid();
            var request = new EmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = DateTime.Today.AddYears(-18)
            };
            var exceptionMessage = "Gender cannot be empty.";

            //Act
            var response = _service.Update(employeeId, request, CancellationToken.None);

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => response);
            Assert.Equal(exceptionMessage, exception.Message);
        }

        [Fact]
        public async Task Update_ShouldReturnException_WhenTitleIdIsNullOrEmpty()
        {
            //Arrange
            var employeeId = Guid.NewGuid();
            var request = new EmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = DateTime.Today.AddYears(-18),
                Gender = "Male"
            };
            var exceptionMessage = "TitleId cannot be empty.";

            //Act
            var response = _service.Update(employeeId, request, CancellationToken.None);

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => response);
            Assert.Equal(exceptionMessage, exception.Message);
        }

        [Fact]
        public async Task Update_ShouldReturnUpdatedEmployee_WhenRequestDataIsOk()
        {
            //Arrange
            var employeeId = Guid.NewGuid();
            var request = new EmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = DateTime.Today.AddYears(-18),
                Gender = "Male",
                TitleId = Guid.NewGuid()
            };
            var title = new Title
            {
                Id = request.TitleId,
                Description = "description"
            };
            var employee = new Employee {
                Id = employeeId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate,
                Gender = request.Gender,
                TitleId = request.TitleId,
                TitleDescription = title.Description
            };

            _repository.Get(employeeId, Arg.Any<CancellationToken>()).Returns(employee);

            //Act
            var response = await _service.Update(employeeId, request, CancellationToken.None);

            //Assert
            Assert.Equal(employeeId, response.Id);
            Assert.Equal(request.FirstName, response.FirstName);
            Assert.Equal(request.LastName, response.LastName);
            Assert.Equal(request.BirthDate, response.BirthDate);
            Assert.Equal(request.Gender, response.Gender);
            Assert.Equal(request.TitleId, response.TitleId);
        }
    }
}