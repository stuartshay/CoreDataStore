using System;

namespace CoreDataStore.Service.Models
{
    public class LPCReportModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string LPCId { get; set; }

        public string LPNumber { get; set; }

        public string ObjectType { get; set; }

        public string Architect { get; set; }

        public string Style { get; set; }

        public string Street { get; set; }

        public string Borough { get; set; }

        public DateTime DateDesignated { get; set; }

        public bool PhotoStatus { get; set; }

        public string PhotoURL { get; set; }

  

    }
}
