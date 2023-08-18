using System.Net.Http.Headers;
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
            _client = factory.HttpClient;
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

        public async Task AuthenticateAsync()
        {
            _client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
            var response = await _client.PostAsJsonAsync("register", new UserRegistrationRequest
            {
                Email = "test@integration.com",
                Password = "SomePass1234!"
            });

            var registrationResponse = await response.Content.ReadFromJsonAsync<AuthSuccessResponse>();

            return registrationResponse.Token;
        }
    }
}