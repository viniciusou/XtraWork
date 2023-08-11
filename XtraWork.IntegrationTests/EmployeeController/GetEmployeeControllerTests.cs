using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using XtraWork.API.Entities;
using XtraWork.API.Repositories;
using XtraWork.API.Requests;

namespace XtraWork.IntegrationTests.EmployeeController
{
    [Collection("Test collection")]
    public class GetEmployeeControllerTests : IAsyncLifetime
    {
        private readonly HttpClient _client;
        private readonly Func<Task> _resetDatabase;
        private readonly TestsSeed _testsSeed;

        public GetEmployeeControllerTests(IntegrationTestFactory<Program, XtraWorkContext> factory)
        {
            _client = factory.CreateClient();
            _resetDatabase = factory.ResetDatabaseAsync;
            _testsSeed = new TestsSeed(factory);
        }

        [Fact]
        public async Task Get_ShouldReturnNotFound_WhenEmployeeDoesNotExistInDatabase()
        {
            //Arrange
            var unknownEmployeeId = Guid.NewGuid();
            
            //Act
            var response = await _client.GetAsync($"employees/{unknownEmployeeId}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_ShouldReturnEmployee_WhenEmployeeExistsInDatabase()
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

            //Act
            var response = await _client.GetAsync($"employees/{createdEmployee.Id}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedEmployee = await response.Content.ReadFromJsonAsync<Employee>();
            returnedEmployee.Id.Should().Be(createdEmployee.Id);
            returnedEmployee.FirstName.Should().Be(createdEmployee.FirstName);
            returnedEmployee.LastName.Should().Be(createdEmployee.LastName);
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