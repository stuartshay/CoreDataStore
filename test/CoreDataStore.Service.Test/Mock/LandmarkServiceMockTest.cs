using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using CoreDataStore.Data.Interfaces;
using CoreDataStore.Domain.Entities;
using CoreDataStore.Service.Services;
using CoreDataStore.Service.Test.Data;
using Moq;
using Xunit;

namespace CoreDataStore.Service.Test.Mock
{
    public class LandmarkServiceMockTest
    {
        [Fact(DisplayName = "Get Landmark Street List")]
        [Trait("Category", "Unit")]
        public async Task Get_Landmark_Item_Street_List()
        {
            var dataSet = LandmarkDataSource.GetLandmarkList(20);

            var repository = new Mock<ILandmarkRepository>();
            repository.Setup(b => b.FindByAsync(It.IsAny<Expression<Func<Landmark, bool>>>()))
                .ReturnsAsync(dataSet);

            var service = GetLandmarkService(repository.Object);

            // Act
            string id = Guid.NewGuid().ToString();
            var sut = await service.GetLandmarkStreetsAsync(id);

            // Assert
            Assert.NotNull(sut);
            Assert.Equal(dataSet.Count, sut.Count);
            Assert.IsType<List<string>>(sut);
        }

        private LandmarkService GetLandmarkService(ILandmarkRepository landmarkRepository = null)
        {
            var config = new MapperConfiguration(cfg => { cfg.AddMaps("CoreDataStore.Service"); });
            IMapper mapper = new Mapper(config);

            landmarkRepository ??= new Mock<ILandmarkRepository>().Object;
            return new LandmarkService(landmarkRepository, mapper);
        }
    }
}
