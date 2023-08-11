using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using XtraWork.API.Entities;
using XtraWork.API.Repositories;
using XtraWork.API.Requests;

namespace XtraWork.IntegrationTests.EmployeeController
{
    [Collection("Test collection")]
    public class SearchEmployeeControllerTests : IAsyncLifetime
    {
        private readonly HttpClient _client;
        private readonly Func<Task> _resetDatabase;
        private readonly TestsSeed _testsSeed;

        public SearchEmployeeControllerTests(IntegrationTestFactory<Program, XtraWorkContext> factory)
        {
            _client = factory.HttpClient;
            _resetDatabase = factory.ResetDatabaseAsync;
            _testsSeed = new TestsSeed(factory);
        }

        [Fact]
        public async Task Search_ShouldReturnNotFound_WhenNoEmployeeIsFoundInDatabaseForKeywordSearched()
        {
            //Arrange
            var keyword = "unknown";
            
            //Act
            var response = await _client.GetAsync($"employees/search?keyword={keyword}");
            
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Search_ShouldReturnEmployeeList_WhenEmployeesAreFoundInDatabaseForKeywordSearched()
        {
            //Arrange
            var keyword = "jo";

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
            
            await _testsSeed.CreateEmployeeAsync(secondEmployeeRequest);
            
            //Act
            var response = await _client.GetAsync($"employees/search?keyword={keyword}");
            
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedEmployeeList = await response.Content.ReadFromJsonAsync<List<Employee>>();            
            returnedEmployeeList.Count().Should().Be(1);
            returnedEmployeeList[0].FirstName.Should().Be(firstCreatedEmployee.FirstName);
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