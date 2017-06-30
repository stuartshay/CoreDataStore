using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreDataStore.Data.SqlServer
{
    public interface IEntityTypeConfiguration<T> where T : class
    {
        void Configure(EntityTypeBuilder<T> builder);
    }
}
