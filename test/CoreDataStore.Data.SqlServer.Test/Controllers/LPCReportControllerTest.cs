using System;
using System.Collections.Generic;
using CoreDataStore.Data.Interfaces;
using CoreDataStore.Data.SqlServer.Repositories;
using CoreDataStore.Domain.Entities;
using CoreDataStore.Service.Interfaces;
using CoreDataStore.Service.Mappings;
using CoreDataStore.Service.Services;
using CoreDataStore.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;
using System.Linq;
using CoreDataStore.Domain.Enum;
using CoreDataStore.Web.Filters;
using CoreDataStore.Service.Models;
using GenFu;
using Navigator.Common.Helpers;

namespace CoreDataStore.Data.SqlServer.Test.Controllers
{
    public class LPCReportControllerTest
    {
        private readonly IServiceProvider _serviceProvider;

        public LPCReportControllerTest()
        {
            var services = new ServiceCollection();
            services.AddDbContext<NYCLandmarkContext>(options => options.UseInMemoryDatabase());

            //Repositories
            services.AddScoped<ILPCReportRepository, LPCReportRepository>();
            services.AddScoped<ILandmarkRepository, LandmarkRepository>();

            //Services
            services.AddScoped<ILPCReportService, LPCReportService>();
            services.AddScoped<ILandmarkService, LandmarkService>();

            AutoMapperConfiguration.Configure();

            _serviceProvider = services.BuildServiceProvider();
        }

        private void CreateTestData(NYCLandmarkContext dbContext)
        {
            var i = 0;
            var boroughs = EnumHelper.EnumToList<Borough>().Select(e => e.GetDescription());
            var boroughCodes = EnumHelper.EnumToList<Borough>().Select(e => e.GetAttribute<BoroughId>().Value);
            var objectTypes = EnumHelper.EnumToList<ObjectType>().Select(e => e.GetDescription());

            GenFu.GenFu.Configure<LPCReport>()
                .Fill(l => l.Id, () => ++i)
                .Fill(l => l.LPNumber, () => string.Format("LP-{0,5:D5}", i))
                .Fill(l => l.LPCId, () => string.Format("{0,5:D5}", i))
                .Fill(l => l.Borough, () => BaseValueGenerator.GetRandomValue(boroughs))
                .Fill(l => l.ObjectType, () => BaseValueGenerator.GetRandomValue(objectTypes));

            var lpcReports = GenFu.GenFu.ListOf<LPCReport>(20);

            dbContext.LPCReports.AddRange(lpcReports);

            var j = 0;
            GenFu.GenFu.Configure<Landmark>()
                .Fill(m => m.Id, () => ++j)
                .Fill(l => l.LM_TYPE, () => BaseValueGenerator.GetRandomValue(objectTypes))
                .Fill(p => p.Pluto)
                .Fill(l => l.BoroughID, () => BaseValueGenerator.GetRandomValue(boroughCodes))
                .Fill(m => m.LP_NUMBER, () => string.Format("LP-{0,5:D5}", j));


            var landmarks = GenFu.GenFu.ListOf<Landmark>(20);

            dbContext.Landmarks.AddRange(landmarks);
            dbContext.SaveChanges();
        }

        private LPCReportController PrepareController()
        {
            var dbContext = _serviceProvider.GetRequiredService<NYCLandmarkContext>();
            dbContext.Database.EnsureDeleted();

            CreateTestData(dbContext);

            var lpcReportSvc = _serviceProvider.GetRequiredService<ILPCReportService>();
            var landmarkSvc = _serviceProvider.GetRequiredService<ILandmarkService>();

            var controller = new LPCReportController(lpcReportSvc, landmarkSvc);

            return controller;
        }


        [Fact(Skip = "ci test")]
        public void DbContext_Should_Have_Records()
        {
            var controller = PrepareController();
            var dbContext = _serviceProvider.GetRequiredService<NYCLandmarkContext>();

            Assert.Equal(20, dbContext.LPCReports.ToList().Count());
            Assert.Equal(20, dbContext.Landmarks.ToList().Count());
        }


        [Fact(Skip = "ci test")]
        public void Get_Should_Have_Record()
        {
            var controller = PrepareController();

            var id = 10;
            var actionResult = controller.Get(id);

            // Assert
            actionResult.Should().BeOfType<ObjectResult>();

            //// Assert
            //actionResult.Should().BeOfType<ObjectResult>()
            //    .Which.Should().BeOfType<LPCReportModel>()
            //    .Which.Id.Should().Be(id);
        }


        [Fact(Skip = "ci test")]
        public void Get_Should_Return_BadRequest()
        {
            var controller = PrepareController();

            var actionResult = controller.Get(0);

            // Assert
            actionResult.Should().BeOfType<BadRequestResult>();
        }


        [Fact(Skip = "ci test")]
        public void Paging_Should_Return_Correct_Record_Count()
        {
            var controller = PrepareController();

            var model = new LPCReportRequestModel
            {
                ObjectType = "Individual Landmark",
            };

            //Response Header Null Exception
            var actionResult = controller.Get(model, 5, 1);

            // Assert
            actionResult.Should().BeOfType<IEnumerable<ObjectResult>>()
                               .Which.Count().Should().Be(5);

        }


        [Fact(Skip = "ci test")]
        public void Paging_Should_Return_No_Record()
        {
            var controller = PrepareController();

            var model = new LPCReportRequestModel
            {
                ObjectType = "ObjectType45",
            };

            var actionResult = controller.Get(model, 5, 1);

            // Assert
            actionResult.Should().BeOfType<IEnumerable<LPCReportModel>>()
                .Which.Count().Should().Equals(0);
        }


        [Fact(Skip = "ci test")]
        public void Put_Exception_Return_NoContent()
        {
            var controller = PrepareController();

            var id = 110;

            var dbContext = _serviceProvider.GetRequiredService<NYCLandmarkContext>();
            var model1 = dbContext.LPCReports.Where(x => x.Id == 1);

            var model = new LPCReportModel { };

            var actionResult = controller.Put(id, model);

            // Assert
            actionResult.Should().BeOfType<NoContentResult>();
        }


        [Fact(Skip = "ci test")]
        public void Put_Valiation_Return_Name_Error()
        {

        }


        [Fact(Skip = "ci test")]
        public void Put_Sucess_Update_Record()
        {
            var controller = PrepareController();

            var id = 11;
            var model = new LPCReportModel
            {

            };

            var actionResult = controller.Put(id, model);

            // Assert
            actionResult.Should().BeOfType<OkResult>();
        }


    }
}
