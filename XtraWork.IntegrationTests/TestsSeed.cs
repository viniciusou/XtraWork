using System.Net.Http.Json;
using XtraWork.API.Repositories;
using XtraWork.API.Requests;
using XtraWork.API.Responses;

namespace XtraWork.IntegrationTests
{
    [CollectionDefinition("Test collection")]
    public class TestsSeed
    {
        private readonly HttpClient _client;

        public TestsSeed(IntegrationTestFactory<Program, XtraWorkContext> factory)
        {
            _client = factory.CreateClient();
        }

        public async Task<TitleResponse> CreateTitleAsync(TitleRequest request)
        {
            var response = await _client.PostAsJsonAsync("titles", request);
            return await response.Content.ReadFromJsonAsync<TitleResponse>();
        }

        public async Task<EmployeeResponse> CreateEmployeeAsync(EmployeeRequest request)
        {
            var response = await _client.PostAsJsonAsync("employees", request);
            return await response.Content.ReadFromJsonAsync<EmployeeResponse>();
        }   
    }
}