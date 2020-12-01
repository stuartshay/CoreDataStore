using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreDataStore.Data.Interfaces;
using CoreDataStore.Data.SqlServer.Repositories;
using CoreDataStore.Data.SqlServer.Test.Helpers;
using CoreDataStore.Domain.Entities;
using CoreDataStore.Domain.Enum;
using CoreDataStore.Service.Interfaces;
using CoreDataStore.Service.Mappings;
using CoreDataStore.Service.Models;
using CoreDataStore.Service.Services;
using CoreDataStore.Web.Controllers;
using CoreDataStore.Web.Filters;
using FluentAssertions;
using GenFu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Navigator.Common.Helpers;
using Xunit;

namespace CoreDataStore.Data.SqlServer.Test.Controllers
{
    public class LpcReportControllerTest
    {
        private readonly IServiceProvider _serviceProvider;

        private IEnumerable<LpcReport> _lpcReports = new List<LpcReport>();
        private readonly IEnumerable<Landmark> _landmarks = new List<Landmark>();

        public LpcReportControllerTest()
        {
            var services = new ServiceCollection();

#warning COMMENT OUT
            //services.AddDbContext<NycLandmarkContext>(options => options.UseInMemoryDatabase());

            // Repositories
            services.AddScoped<ILpcReportRepository, LpcReportRepository>();
            services.AddScoped<ILandmarkRepository, LandmarkRepository>();

            // Services
            services.AddScoped<ILpcReportService, LpcReportService>();
            services.AddScoped<ILandmarkService, LandmarkService>();

            AutoMapperConfiguration.Configure();
            _serviceProvider = services.BuildServiceProvider();
        }

        private void CreateTestData(NycLandmarkContext dbContext)
        {
            var boroughs = EnumHelper.EnumToList<Borough>().Select(e => e.GetDescription());
            var objectTypes = EnumHelper.EnumToList<ObjectType>().Select(e => e.GetDescription());

            var i = 0;
            GenFu.GenFu.Configure<LpcReport>()
                .Fill(l => l.Id, () => ++i)
                .Fill(l => l.LPNumber, () => $"LP-{i,5:D5}")
                .Fill(l => l.LPCId, () => $"{i,5:D5}")
                .Fill(l => l.Borough, () => BaseValueGenerator.GetRandomValue(boroughs))
                .Fill(l => l.ObjectType, () => BaseValueGenerator.GetRandomValue(objectTypes))
                .Fill(l => l.Landmarks);

            _lpcReports = GenFu.GenFu.ListOf<LpcReport>(20);

            dbContext.LPCReports.AddRange(_lpcReports);
            dbContext.SaveChanges();
        }

        private LpcReportController PrepareController()
        {
            var dbContext = _serviceProvider.GetRequiredService<NycLandmarkContext>();
            dbContext.Database.EnsureDeleted();

            CreateTestData(dbContext);

            var lpcReportSvc = _serviceProvider.GetRequiredService<ILpcReportService>();
            var landmarkSvc = _serviceProvider.GetRequiredService<ILandmarkService>();

            var controller = new LpcReportController(lpcReportSvc, landmarkSvc);
            controller.ControllerContext = WebTestHelpers.GetHttpContext();

            return controller;
        }

        [Fact]
        [Trait("Category", "InMemoryDatabase")]
        public void DbContext_Should_Have_Records()
        {
            var controller = PrepareController();
            var dbContext = _serviceProvider.GetRequiredService<NycLandmarkContext>();

            Assert.Equal(20, dbContext.LPCReports.ToList().Count);
        }

        [Fact]
        [Trait("Category", "InMemoryDatabase")]
        public async Task Get_Should_Have_Record()
        {
            var controller = PrepareController();

            var id = 10;
            var actionResult = await controller.Get(id);

            // Assert
            actionResult.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        [Trait("Category", "InMemoryDatabase")]
        public async Task Get_Should_Return_BadRequest()
        {
            var controller = PrepareController();

            var actionResult = await controller.Get(0);

            // Assert
            actionResult.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        [Trait("Category", "InMemoryDatabase")]
        public void Paging_Should_Return_Correct_Record_Count()
        {
            var controller = PrepareController();

            var model = new LPCReportRequestModel
            {
                ObjectType = "Individual Landmark",
            };

            // Act
            var actionResult = controller.Get(model, 5, 1);

            // Assert
            actionResult.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        [Trait("Category", "InMemoryDatabase")]
        public void Paging_Should_Return_No_Record()
        {
            var controller = PrepareController();

            var model = new LPCReportRequestModel
            {
                ObjectType = "ObjectType45",
            };

            var actionResult = controller.Get(model, 5, 1);

            // Assert
            actionResult.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        [Trait("Category", "InMemoryDatabase")]
        public void Put_Exception_Return_NotFoundResult()
        {
            var controller = PrepareController();

            // Act
            var id = 110;
            var model = new LpcReportModel { };
            var actionResult = controller.Put(id, model);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        [Trait("Category", "InMemoryDatabase")]
        public void Put_Success_Update_Record()
        {
            var controller = PrepareController();

            var id = 11;
            var model = new LpcReportModel
            {
                Id = 11,
                Architect = "Test",
                DateDesignated = DateTime.UtcNow,
                Borough = "X",
            };

            var actionResult = controller.Put(id, model);

            // Assert
            actionResult.Should().BeOfType<NoContentResult>();
        }
    }
}
