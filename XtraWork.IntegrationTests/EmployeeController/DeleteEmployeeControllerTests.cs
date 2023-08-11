using System.Net;
using FluentAssertions;
using XtraWork.API.Repositories;
using XtraWork.API.Requests;

namespace XtraWork.IntegrationTests.EmployeeController
{
    [Collection("Test collection")]
    public class DeleteEmployeeControllerTests : IAsyncLifetime
    {
        private readonly HttpClient _client;
        private readonly Func<Task> _resetDatabase;
        private readonly TestsSeed _testsSeed;

        public DeleteEmployeeControllerTests(IntegrationTestFactory<Program, XtraWorkContext> factory)
        {
            _client = factory.HttpClient;
            _resetDatabase = factory.ResetDatabaseAsync;
            _testsSeed = new TestsSeed(factory);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenEmployeeDoesNotExistInDatabase()
        {
            //Arrange
            var unknownEmployeeId = Guid.NewGuid();
            
            //Act
            var response = await _client.DeleteAsync($"employees/{unknownEmployeeId}");
            
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);            
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenEmployeeExistsInDatabase()
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
            var response = await _client.DeleteAsync($"employees/{createdEmployee.Id}");
            
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);            
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