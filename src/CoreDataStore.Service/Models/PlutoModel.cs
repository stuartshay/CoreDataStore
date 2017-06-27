namespace CoreDataStore.Service.Models
{
    public class PlutoModel
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

        public double XCoord { get; set; }

        public double YCoord { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
