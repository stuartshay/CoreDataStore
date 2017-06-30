using CoreDataStore.Domain.Entities.Base;

namespace CoreDataStore.Domain.Entities
{
    public class LPCLamppost : IEntityBase, IAuditableEntity
    {
        public long Id { get; set; }

        public string Type { get; set; }

        public string SubType { get; set; }

        public int? Block { get; set; }

        public int? Lot { get; set; }

        public string Borough { get; set; }

        public string Located { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

    }


}
