using XtraWork.API.Repositories;

namespace XtraWork.IntegrationTests
{
    [CollectionDefinition("Test collection")]
    public class SharedTestCollection : ICollectionFixture<IntegrationTestFactory<Program, XtraWorkContext>>
    {
        
    }
}