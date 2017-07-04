using CoreDataStore.Domain.Entities.Base;

namespace CoreDataStore.Domain.Entities
{
    public class LPCLocation : IEntityBase, IAuditableEntity
    {
        public long Id { get; set; }

        public string LPNumber { get; set; }

        public string Name { get; set; }

        public long? BBL { get; set; }

        public string ObjectType { get; set; }

        public string Street { get; set; }

        public string Address { get; set; }

        public string Neighborhood { get; set; }

        public string Borough { get; set; }

        public string ZipCode { get; set; }

        public int? Block { get; set; }

        public int? Lot { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

    }
}
