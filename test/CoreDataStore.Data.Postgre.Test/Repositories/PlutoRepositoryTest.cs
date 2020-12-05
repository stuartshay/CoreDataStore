using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreDataStore.Data.Interfaces;
using CoreDataStore.Data.Postgre.Test.Fixtures;
using CoreDataStore.Domain.Entities;
using Xunit;
using Xunit.Abstractions;

namespace CoreDataStore.Data.Postgre.Test.Repositories
{
    public class PlutoRepositoryTest : IClassFixture<CoreDataStoreDbFixture>
    {
        private readonly IPlutoRepository _plutoRepository;

        private readonly ITestOutputHelper _output;

        public PlutoRepositoryTest(CoreDataStoreDbFixture fixture, ITestOutputHelper output)
        {
            _plutoRepository = fixture.PlutoRepository;
            _output = output;
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void Pluto_Block_Lot_Exist()
        {
            int block = 1096;
            int lot = 54;

            var result = _plutoRepository.FindBy(x => x.Block == block && x.Lot == lot).FirstOrDefault();

            Assert.NotNull(result);
            Assert.Equal(block, result.Block);
            Assert.Equal(lot, result.Lot);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void Pluto_BBL_Exist()
        {
            long bbl = 5080500013;
            var result = _plutoRepository.FindBy(x => x.BBL == bbl).FirstOrDefault();

            Assert.NotNull(result);
            Assert.Equal("10307", result.ZipCode);
            Assert.Equal(bbl, bbl);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void Get_Pluto_Item_By_LpcNumber()
        {
            var lpcNumber = "LP-02039";
            var result = _plutoRepository.GetPluto(lpcNumber);

            Assert.NotNull(result);
            Assert.IsType<List<Pluto>>(result);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task Get_Pluto_Item_By_LpcNumber_Async()
        {
            var lpcNumber = "LP-02039";
            var result = await _plutoRepository.GetPlutoAsync(lpcNumber);

            Assert.NotNull(result);
            Assert.IsType<List<Pluto>>(result);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void Get_Pluto_Item_By_LpcNumber_Count()
        {
            var lpcNumber = "LP-02039";
            var result = _plutoRepository.GetPlutoCount(lpcNumber);

            Assert.True(result > 0);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task Get_Landmarks_Pluto_Async()
        {
            var lpcNumber = "LP-02039";
            var results = await _plutoRepository.GetPlutoAsync(lpcNumber);

            Assert.NotNull(results);
            Assert.IsType<List<Pluto>>(results);

            var item = results.First();
            Assert.IsType<Pluto>(item);
        }

        [Fact(DisplayName = "Pluto Count - Async")]
        [Trait("Category", "Integration")]
        public async Task Get_Landmarks_Pluto_Count_Async()
        {
            var lpcNumber = "LP-02039";
            var sut = await _plutoRepository.GetPlutoCountAsync(lpcNumber);

            Assert.NotEqual(0, sut);
        }

        [Fact(DisplayName = "Pluto BBL List - Async")]
        [Trait("Category", "Integration")]
        public async Task Get_Pluto_BBL_List_Async()
        {
            long[] bbl = { 5000140088, 5000140016 };

            var sut = await _plutoRepository.FindByAsync(x => bbl.Contains(x.BBL));

            Assert.NotEmpty(sut);
        }
    }
}
