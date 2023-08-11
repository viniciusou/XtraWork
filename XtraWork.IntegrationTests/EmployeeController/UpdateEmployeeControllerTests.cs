using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using XtraWork.API.Entities;
using XtraWork.API.Repositories;
using XtraWork.API.Requests;

namespace XtraWork.IntegrationTests.EmployeeController
{
    [Collection("Test collection")]
    public class UpdateEmployeeControllerTests : IAsyncLifetime
    {
        private readonly HttpClient _client;
        private readonly Func<Task> _resetDatabase;
        private readonly TestsSeed _testsSeed;

        public UpdateEmployeeControllerTests(IntegrationTestFactory<Program, XtraWorkContext> factory)
        {
            _client = factory.HttpClient;
            _resetDatabase = factory.ResetDatabaseAsync;
            _testsSeed = new TestsSeed(factory);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenRequestDataIsMissing()
        {
            //Arrange
            var updateEmployeeId = Guid.NewGuid;

            var updateEmployeeRequest = new EmployeeRequest
            {
                FirstName = string.Empty,
                LastName = "Doe",
                BirthDate = DateTime.Today.AddYears(-18),
                Gender = "Male",
                TitleId = Guid.NewGuid()
            };
            
            //Act
            var response = await _client.PutAsJsonAsync($"employees/{updateEmployeeId}", updateEmployeeRequest);
            
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenEmployeeDoesNotExistInDatabase()
        {
            //Arrange
            var updateEmployeeId = Guid.NewGuid();

            var updateEmployeeRequest = new EmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = DateTime.Today.AddYears(-18),
                Gender = "Male",
                TitleId = Guid.NewGuid()
            };
            
            //Act
            var response = await _client.PutAsJsonAsync($"employees/{updateEmployeeId}", updateEmployeeRequest);
            
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_ShouldReturnOk_WhenEmployeeExistsInDatabase()
        {
            //Arrange
            var titleRequest = new TitleRequest
            {
                Description = "description"
            };

            var createdTitle = await _testsSeed.CreateTitleAsync(titleRequest);

            var employeeRequest = new EmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = DateTime.Today.AddYears(-18),
                Gender = "Male",
                TitleId = createdTitle.Id
            };

            var createdEmployee = await _testsSeed.CreateEmployeeAsync(employeeRequest);

            var updateEmployeeId = createdEmployee.Id;

            var updateEmployeeRequest = new EmployeeRequest
            {
                FirstName = "Joe",
                LastName = "Doe",
                BirthDate = DateTime.Today.AddYears(-19),
                Gender = "Male",
                TitleId = Guid.NewGuid()
            };
            
            //Act
            var response = await _client.PutAsJsonAsync($"employees/{updateEmployeeId}", updateEmployeeRequest);
            
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedEmployee = await response.Content.ReadFromJsonAsync<Employee>();
            returnedEmployee.FirstName.Should().Be(updateEmployeeRequest.FirstName);
            returnedEmployee.BirthDate.Should().Be(updateEmployeeRequest.BirthDate);
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;  
        }

        public Task DisposeAsync()
        {
            return _resetDatabase();
        }
    }
}