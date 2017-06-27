using CoreDataStore.Domain.Entities.Base;

namespace CoreDataStore.Domain.Entities
{
    public class Pluto : IEntityBase
    {
        public long Id { get; set; }

        public string Borough { get; set; }

        public int Block { get; set; }

        public int Lot { get; set; }

        public string Address { get; set; }

        public string OwnerName { get; set; }

        public int LotArea { get; set; }

        public int NumBldgs { get; set; }

        public int YearBuilt { get; set; }

        public string HistDist { get; set; }

        public long BBL { get; set; }

        public string ZipCode { get; set; }

        public int XCoord { get; set; }

        public int YCoord { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public Landmark Landmark { get; set; }

    }
}
