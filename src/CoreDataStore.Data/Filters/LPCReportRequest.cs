﻿using System.Collections.Generic;
using CoreDataStore.Data.Filters.Base;

namespace CoreDataStore.Data.Filters
{
    public class LpcReportRequest : RequestBase
    {
        /// <summary>
        /// Object Type
        /// </summary>
        public string ObjectType { get; set; }

        /// <summary>
        /// Borough
        /// </summary>
        public string Borough { get; set; }

        /// <summary>
        /// Neighborhood
        /// </summary>
        public string Neighborhood { get; set; }

        /// <summary>
        /// Parent Style List
        /// </summary>
        public List<string> ParentStyleList { get; set; }
    }
}
