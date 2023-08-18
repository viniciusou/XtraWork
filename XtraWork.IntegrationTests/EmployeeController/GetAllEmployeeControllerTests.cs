using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using XtraWork.API.Entities;
using XtraWork.API.Repositories;
using XtraWork.API.Requests;
using XtraWork.API.Responses;

namespace XtraWork.IntegrationTests.EmployeeController
{
    [Collection("Test collection")]
    public class GetAllEmployeeControllerTests : IAsyncLifetime
    {
        private readonly HttpClient _client;
        private readonly Func<Task> _resetDatabase;
        private readonly TestsSeed _testsSeed;

        public GetAllEmployeeControllerTests(IntegrationTestFactory<Program, XtraWorkContext> factory)
        {
            _client = factory.HttpClient;
            _resetDatabase = factory.ResetDatabaseAsync;
            _testsSeed = new TestsSeed(factory);
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmptyResponse_WhenThereAreNoEmployeesInDatabase()
        {
            //Arrange
            await _testsSeed.AuthenticateAsync();

            //Act
            var response = await _client.GetAsync("employees");
            
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedEmployeeList = await response.Content.ReadFromJsonAsync<List<Employee>>();            
            returnedEmployeeList.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmployeeListResponse_WhenThereAreEmployeesInDatabase()
        {
            //Arrange
            await _testsSeed.AuthenticateAsync();

            var titleRequest = new TitleRequest
            {
                Description = "description"
            };

            var createdTitle = await _testsSeed.CreateTitleAsync(titleRequest);

            var firstEmployeeRequest = new EmployeeRequest
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = DateTime.Today.AddYears(-18),
                Gender = "Male",
                TitleId = createdTitle.Id
            };
            
            var firstCreatedEmployee = await _testsSeed.CreateEmployeeAsync(firstEmployeeRequest);

            var secondEmployeeRequest = new EmployeeRequest
            {
                FirstName = "Mary",
                LastName = "Jane",
                BirthDate = DateTime.Today.AddYears(-18),
                Gender = "Female",
                TitleId = createdTitle.Id
            };
            
            var secondCreatedEmployee = await _testsSeed.CreateEmployeeAsync(secondEmployeeRequest);
            
            //Act
            var response = await _client.GetAsync("employees");
            
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedEmployeeList = await response.Content.ReadFromJsonAsync<List<Employee>>();            
            returnedEmployeeList.Count().Should().Be(2);
            returnedEmployeeList[0].Id.Should().Be(firstCreatedEmployee.Id);
            returnedEmployeeList[1].Id.Should().Be(secondCreatedEmployee.Id);
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