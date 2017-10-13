using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using TestServerPOC;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using TestServerPOC.Data;
using TestServerPOC.Models;

namespace XUnitTestProject1
{
    public class ApplicationUsersControllerGetApplicationUser
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _client;

        public ApplicationUsersControllerGetApplicationUser()
        {
            var builder = new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<Startup>();

            var server = new TestServer(builder);
            _context = server.Host.Services.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
            _client = server.CreateClient();
        }

        [Fact]
        public async Task DoesReturnNotFound_GivenUserDoesNotExist()
        {
            // Act
            var response = await _client.GetAsync($"/api/ApplicationUsers/abc"); // No users with ID abc

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DoesReturnOk_GivenUserExists()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = "123"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            // Act
            var response = await _client.GetAsync($"/api/ApplicationUsers/{user.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
