namespace Rafting.Infrastructure.Database.Configurations.Identity;

public sealed class UserEntityConfiguration : IEntityTypeConfiguration<RaftingUserEntity>
{
    public void Configure(EntityTypeBuilder<RaftingUserEntity> b)
    {
        b.ToTable("Users");

        b.HasKey(x => x.Id);

        // RegisterCommandHandler checks the email first and returns 409; this index is the last guard.
        // Unique only among non-deleted users: a soft-deleted user keeps its row,
        // and without the filter its email could never be registered again.
        b.HasIndex(x => x.Email)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        b.Property(x => x.Firstname)
            .IsRequired()
            .HasMaxLength(RaftingUserEntity.Constraints.FirstnameMaxLength);

        b.Property(x => x.Lastname)
            .IsRequired()
            .HasMaxLength(RaftingUserEntity.Constraints.LastnameMaxLength);

        b.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(RaftingUserEntity.Constraints.EmailMaxLength);

        b.Property(x => x.PasswordHash)
            .IsRequired();

        b.Property(x => x.Address)
            .HasMaxLength(RaftingUserEntity.Constraints.AddressMaxLength);

        b.Property(x => x.City)
            .HasMaxLength(RaftingUserEntity.Constraints.CityMaxLength);

        b.Property(x => x.Phone)
         .HasMaxLength(RaftingUserEntity.Constraints.PhoneMaxLength);

        // Owner/employee belong to one agency. Restrict: deleting an agency must never delete its users.
        b.HasOne(x => x.Agency)
            .WithMany()
            .HasForeignKey(x => x.AgencyId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.AgencyId);

        // Roles
        b.Property(x => x.IsAdmin)
            .HasDefaultValue(false);

        b.Property(x => x.IsManager)
            .HasDefaultValue(false);

        // No HasDefaultValue(true) for IsEmployee/IsEnabled: EF Core does not send a bool that has the
        // CLR default (false) when the column has a database default, so "false" could never be saved -
        // the database would silently store "true". Defaults belong in the entity (IsEnabled = true).

        b.Property(x => x.TokenVersion)
            .HasDefaultValue(0);

        // Navigation
        b.HasMany(x => x.RefreshTokens)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
    }
}
