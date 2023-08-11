using System.Data.Common;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Respawn.Graph;
using Testcontainers.MsSql;
using XtraWork.API.Repositories;

namespace XtraWork.IntegrationTests
{
    public class IntegrationTestFactory<TProgram, TDbContext> : WebApplicationFactory<TProgram>, IAsyncLifetime
    where TProgram : class where TDbContext : DbContext
    {
        private readonly MsSqlContainer _container;

        public IntegrationTestFactory()
        {
            _container = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPassword("Str0ngPassw0rd!")
                .WithCleanUp(true)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
                .Build();
        }

        private DbConnection _dbConnection = default!;
        private Respawner _respawner = default!;

        public HttpClient HttpClient { get; private set; } = default!;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveDbContext<XtraWorkContext>();
                services.AddDbContext<XtraWorkContext>(options => { options.UseSqlServer(_container.GetConnectionString()); });
                services.EnsureDbCreated<XtraWorkContext>();
            });
        }

        public async Task ResetDatabaseAsync()
        {
            await _respawner.ResetAsync(_dbConnection);
        }

        public async Task InitializeAsync()
        {
            await _container.StartAsync();
            _dbConnection = new SqlConnection(_container.GetConnectionString());
            HttpClient = CreateClient();
            await InitilizeRespawner();
        }

        public new async Task DisposeAsync() 
        {
            await _container.DisposeAsync();
        }

        private async Task InitilizeRespawner()
        {
            await _dbConnection.OpenAsync();
            _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.SqlServer,
                TablesToIgnore = new Table[]
                {
                    "sysdiagrams",
                    "tblUser",
                    "tblObjectType"
                },
            });
        }
    }
}
