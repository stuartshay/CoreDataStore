using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics;
using CoreDataStore.Web.Controllers;
using CoreDataStore.Web.Test.Fixtures;
using CoreDataStore.Web.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace CoreDataStore.Web.Test.TestServerIntegration
{
    public class DiagnosticsControllerTest : IClassFixture<TestServerFixture>
    {
        private readonly HttpClient _client;

        private readonly ITestOutputHelper _output;

        public DiagnosticsControllerTest(TestServerFixture fixture, ITestOutputHelper output)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
            _client = fixture.Client;
            _output = output;
        }

        [Fact(Skip = "TODO")]
        [Trait("Category", "Integration")]
        public async Task Get_Diagnostics_TestServer()
        {
            // Arrange & Act
            var response = await _client.GetAsync("/api/Diagnostics");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var serverDiagnostics = JsonConvert.DeserializeObject<ServerDiagnostics>(responseString);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.IsType<ServerDiagnostics>(serverDiagnostics);
        }

        [Fact(Skip = "TODO")]
        [Trait("Category", "Integration")]
        public async Task Get_Diagnostics_Status_TestServer()
        {
            // Arrange & Act
            var response = await _client.GetAsync("/api/Diagnostics/status");
            response.EnsureSuccessStatusCode();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(Skip = "TODO")]
        [Trait("Category", "Unit")]
        public void Get_ServerDiagnostics_ReturnsData()
        {
            // Arrange
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            mockEnvironment.Setup(m => m.EnvironmentName).Returns("Testing");

            var diagnosticsController = GetDiagnosticsController(mockEnvironment.Object);

            // Act
            var result = diagnosticsController.Get();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var objectResult = result as OkObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult.StatusCode == 200);
            Assert.NotNull(objectResult.Value);

            var serverDiagnostics = objectResult.Value as ServerDiagnostics;

            Assert.NotNull(serverDiagnostics);
            Assert.IsType<ServerDiagnostics>(serverDiagnostics);
            Assert.Equal("Testing", serverDiagnostics.EnvironmentName);
        }

        [Fact(Skip = "TODO")]
        [Trait("Category", "Unit")]
        public void Get_ServerDiagnostics_Returns_Status()
        {
            // Arrange
            var diagnosticsController = GetDiagnosticsController();

            // Act
            var result = diagnosticsController.Status();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);

            var objectResult = result as OkResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult.StatusCode == 200);
        }

        [Fact(Skip = "FIX")]
        [Trait("Category", "Unit")]
        public void ConfigureServices_Development()
        {
            // Arrange
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            var mockEnvironment = new Mock<IHostingEnvironment>();
            mockEnvironment.Setup(m => m.EnvironmentName).Returns("Development");

            //IServiceCollection services = new ServiceCollection();
            //var target = new Startup(WebTestHelpers.Configuration, mockEnvironment.Object);

            //// Act
            //target.ConfigureDevelopmentServices(services);
            //services.AddTransient<DiagnosticsController>();

            //var serviceProvider = services.BuildServiceProvider();

            //// Assert
            //var controller = serviceProvider.GetService<DiagnosticsController>();
            //Assert.NotNull(controller);
        }

        private DiagnosticsController GetDiagnosticsController(
            IWebHostEnvironment hostingEnvironment = null)
        {
            hostingEnvironment = hostingEnvironment ?? new Mock<IWebHostEnvironment>().Object;
            IMetrics metrics = new Mock<IMetrics>().Object;

            //return new DiagnosticsController(hostingEnvironment, metrics);
            return new DiagnosticsController(hostingEnvironment);
        }
    }
}
