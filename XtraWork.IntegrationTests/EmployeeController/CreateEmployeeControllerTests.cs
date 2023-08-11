using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using XtraWork.API.Entities;
using XtraWork.API.Repositories;
using XtraWork.API.Requests;

namespace XtraWork.IntegrationTests.Controllers
{
    [Collection("Test collection")]
    public class CreateEmployeeControllerTests : IAsyncLifetime
    {
        private readonly HttpClient _client;
        private readonly Func<Task> _resetDatabase;
        private readonly TestsSeed _testsSeed;

        public CreateEmployeeControllerTests(IntegrationTestFactory<Program, XtraWorkContext> factory) 
        {
            _client = factory.HttpClient;
            _resetDatabase = factory.ResetDatabaseAsync;
            _testsSeed = new TestsSeed(factory);
        } 

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenDataRequestIsMissing()
        {
            //Arrange
            var titleRequest = new TitleRequest
            {
                Description = "description"
            };

            var createdTitle = await _testsSeed.CreateTitleAsync(titleRequest);

            var employeeRequest = new EmployeeRequest
            {
                FirstName = null,
                LastName = "Doe",
                BirthDate = DateTime.Today.AddYears(-18),
                Gender = "Male",
                TitleId = createdTitle.Id
            };
            
            //Act
            var response = await _client.PostAsJsonAsync("employees", employeeRequest);
            
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_ShouldReturnEmployee_WhenDataRequestIsComplete()
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
            
            //Act
            var response = await _client.PostAsJsonAsync("employees", employeeRequest);
            
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var returnedEmployee = await response.Content.ReadFromJsonAsync<Employee>();
            returnedEmployee.FirstName.Should().Be(employeeRequest.FirstName);
            returnedEmployee.TitleId.Should().Be(employeeRequest.TitleId);
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