using Market.Domain.Entities.Agencies;

namespace Market.Infrastructure.Database.Configurations.Agencies;

public sealed class AgencyConfiguration : IEntityTypeConfiguration<AgencyEntity>
{
    public void Configure(EntityTypeBuilder<AgencyEntity> builder)
    {
        builder.ToTable("Agencies");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(AgencyEntity.Constraints.NameMaxLength);

        builder.Property(x => x.Slug)
            .IsRequired()
            .HasMaxLength(AgencyEntity.Constraints.SlugMaxLength);

        // Slug je jedinstven samo među neobrisanim agencijama (soft delete).
        builder.HasIndex(x => x.Slug)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasOne(x => x.Owner)
            .WithMany()
            .HasForeignKey(x => x.OwnerUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.DefaultCurrency)
            .WithMany()
            .HasForeignKey(x => x.DefaultCurrencyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}