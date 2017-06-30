using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreDataStore.Data.Conventions
{
    public interface IEntityTypeConfiguration<T> where T : class
    {
        void Configure(EntityTypeBuilder<T> builder);
    }
}
