using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CoreDataStore.Data.Interfaces;
using CoreDataStore.Service.Models;
using CoreDataStore.Service.Services;
using CoreDataStore.Service.Test.Data;
using Moq;
using Xunit;

namespace CoreDataStore.Service.Test.Mock
{
    public class PlutoServiceMockTest
    {
        [Fact(DisplayName = "Get LPC Item Pluto List")]
        [Trait("Category", "Unit")]
        public void Get_LPC_Pluto_Item_Data()
        {
            var dataSet = PlutoDataSource.GetPlutoList(20);

            var plutoRepository = new Mock<IPlutoRepository>();
            plutoRepository.Setup(b => b.GetPluto(It.IsAny<string>()))
                .Returns(dataSet);

            var service = GetPlutoService(plutoRepository.Object);

            // Act
            var id = Guid.NewGuid().ToString();
            var sut = service.GetPluto(id);

            // Assert
            Assert.NotNull(sut);
            Assert.IsType<List<PlutoModel>>(sut);
        }

        [Fact(DisplayName = "Get LPC Item Pluto List - Async")]
        [Trait("Category", "Unit")]
        public async Task Get_LPC_Pluto_Item_Data_Async()
        {
            var dataSet = PlutoDataSource.GetPlutoList(20);

            var plutoRepository = new Mock<IPlutoRepository>();
            plutoRepository.Setup(b => b.GetPluto(It.IsAny<string>()))
                .Returns(dataSet);

            var service = GetPlutoService(plutoRepository.Object);

            // Act
            var id = Guid.NewGuid().ToString();
            var sut = await service.GetPlutoAsync(id);

            // Assert
            Assert.NotNull(sut);
            Assert.IsType<List<PlutoModel>>(sut);
        }

        private PlutoService GetPlutoService(IPlutoRepository plutoRepository = null)
        {
            var config = new MapperConfiguration(cfg => { cfg.AddMaps("CoreDataStore.Service"); });
            IMapper mapper = new Mapper(config);

            plutoRepository ??= new Mock<IPlutoRepository>().Object;
            return new PlutoService(plutoRepository, mapper);
        }
    }
}
