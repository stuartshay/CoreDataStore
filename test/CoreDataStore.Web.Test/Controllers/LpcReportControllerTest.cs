﻿using System.Linq;
using System.Threading.Tasks;
using CoreDataStore.Data.Filters;
using CoreDataStore.Service.Interfaces;
using CoreDataStore.Service.Models;
using CoreDataStore.Service.Test.Data;
using CoreDataStore.Web.Controllers;
using CoreDataStore.Web.Filters;
using CoreDataStore.Web.Test.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CoreDataStore.Web.Test.Controllers
{
    public class LpcReportControllerTest
    {
        [Fact(DisplayName = "LPC Reports - (Returns Data)")]
        [Trait("Category", "Unit")]
        public async Task Get_Lpc_Report_Returns_Data()
        {
            var dataSet = LpcReportDataSource.GetLpcReportModelItem();

            var lpcReportsService = new Mock<ILpcReportService>();
            lpcReportsService.Setup(x => x.GetLPCReportAsync(1))
                .ReturnsAsync(dataSet);

            var controller = GetLpcReportController(lpcReportsService.Object);
            controller.ControllerContext = WebTestHelpers.GetHttpContext();

            // Act
            var sut = await controller.Get(1);

            // Assert
            Assert.NotNull(sut);
            Assert.IsType<OkObjectResult>(sut);

            var objectResult = sut as OkObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult.StatusCode == 200);
        }

        [Fact(DisplayName = "LPC Reports - (Returns Bad Request)")]
        [Trait("Category", "Unit")]
        public async Task Get_Lpc_Report_Returns_Bad_Request()
        {
            var controller = GetLpcReportController();
            controller.ControllerContext = WebTestHelpers.GetHttpContext();

            // Act
            var sut = await controller.Get(0);

            // Assert
            Assert.NotNull(sut);
            Assert.IsType<BadRequestResult>(sut);

            var objectResult = sut as BadRequestResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult.StatusCode == 400);
        }

        [Fact(DisplayName = "LPC Reports - (Returns Not Found)")]
        [Trait("Category", "Unit")]
        public async Task Get_Lpc_Report_Returns_NotFound()
        {
            var lpcReportsService = new Mock<ILpcReportService>();
            lpcReportsService.Setup(x => x.GetLPCReportAsync(1))
               .ReturnsAsync((LpcReportModel)null);

            var controller = GetLpcReportController(lpcReportsService.Object);
            controller.ControllerContext = WebTestHelpers.GetHttpContext();

            // Act
            var sut = await controller.Get(1);

            // Assert
            Assert.NotNull(sut);
            Assert.IsType<NotFoundResult>(sut);

            var objectResult = sut as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult.StatusCode == 404);
        }

        [Fact(DisplayName = "LPC Reports Paging - (Returns Data)")]
        [Trait("Category", "Unit")]
        public void Get_Lpc_Report_Paging_Returns_Data()
        {
            var pageSize = 30;
            var dataSourceResults = new PagedResultModel<LpcReportModel>
            {
                Results = LpcReportDataSource.GetLpcReportModelList(pageSize),
                Page = 1,
                Limit = pageSize,
            };

            var lpcReportsService = new Mock<ILpcReportService>();
            lpcReportsService.Setup(x => x.GetLPCReports(It.IsAny<LpcReportRequest>()))
               .Returns(dataSourceResults);

            var controller = GetLpcReportController(lpcReportsService.Object);
            controller.ControllerContext = WebTestHelpers.GetHttpContext();

            // Act
            var request = new LPCReportRequestModel { };
            var sut = controller.Get(request, pageSize, 1);

            // Assert
            Assert.NotNull(sut);
            Assert.IsType<OkObjectResult>(sut);

            var objectResult = sut as OkObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult.StatusCode == 200);
        }

        [Fact(DisplayName = "Landmarks Paging - (Returns Data)")]
        [Trait("Category", "Unit")]
        public void Get_Landmarks_Paging_Returns_Data()
        {
            var pageSize = 30;
            var dataSourceResults = new PagedResultModel<LandmarkModel>
            {
                Results = LandmarkDataSource.GetLandmarkModelList(pageSize),
                Page = 1,
                Limit = pageSize,
            };

            var landmarkService = new Mock<ILandmarkService>();
            landmarkService.Setup(x => x.GetLandmarks(It.IsAny<LandmarkRequest>()))
                .Returns(dataSourceResults);

            var controller = GetLpcReportController(null, landmarkService.Object);
            controller.ControllerContext = WebTestHelpers.GetHttpContext();

            // Act
            var request = new LandmarkRequestModel { LpcNumber = "XXXXX" };
            var sut = controller.GetLandmarks(request, pageSize, 1);

            // Assert
            Assert.NotNull(sut);
            Assert.IsType<OkObjectResult>(sut);

            var objectResult = sut as OkObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult.StatusCode == 200);
        }

        [Fact(DisplayName = "LPC Streets - (Returns Data)")]
        [Trait("Category", "Unit")]
        public void Get_Lpc_Streets_Returns_Data()
        {
            var dataSet = LpcReportDataSource.GetLpcReportsList(30).Select(x => x.Street).ToList();

            var landmarkService = new Mock<ILandmarkService>();
            landmarkService.Setup(x => x.GetLandmarkStreets(It.IsAny<string>()))
                .Returns(dataSet);

            var controller = GetLpcReportController(null, landmarkService.Object);
            controller.ControllerContext = WebTestHelpers.GetHttpContext();

            // Act
            var lpcId = System.Guid.NewGuid().ToString();
            var sut = controller.GetStreets(lpcId);

            // Assert
            Assert.NotNull(sut);
            Assert.IsType<OkObjectResult>(sut);

            var objectResult = sut as OkObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult.StatusCode == 200);
        }

        [Fact(DisplayName = "LPC Streets - (Not Found)")]
        [Trait("Category", "Unit")]
        public void Get_Lpc_Streets_Returns_Not_Found()
        {
            var controller = GetLpcReportController();

            var sut = controller.GetStreets(null);

            // Assert
            Assert.NotNull(sut);
        }

        private LpcReportController GetLpcReportController(ILpcReportService lpcReportService = null, ILandmarkService landmarkService = null)
        {
            lpcReportService = lpcReportService ?? new Mock<ILpcReportService>().Object;
            landmarkService = landmarkService ?? new Mock<ILandmarkService>().Object;

            return new LpcReportController(lpcReportService, landmarkService);
        }
    }
}
