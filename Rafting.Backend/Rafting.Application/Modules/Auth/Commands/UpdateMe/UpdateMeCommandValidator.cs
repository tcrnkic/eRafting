namespace Rafting.Application.Modules.Auth.Commands.UpdateMe;

/// <summary>
/// Same rules for these fields as <see cref="Register.RegisterCommandValidator"/>.
/// </summary>
public sealed class UpdateMeCommandValidator : AbstractValidator<UpdateMeCommand>
{
    public UpdateMeCommandValidator()
    {
        RuleFor(x => x.Firstname)
            .NotEmpty().WithMessage("Ime je obavezno.")
            .MaximumLength(RaftingUserEntity.Constraints.FirstnameMaxLength)
            .WithMessage($"Ime može imati najviše {RaftingUserEntity.Constraints.FirstnameMaxLength} znakova.");

        RuleFor(x => x.Lastname)
            .NotEmpty().WithMessage("Prezime je obavezno.")
            .MaximumLength(RaftingUserEntity.Constraints.LastnameMaxLength)
            .WithMessage($"Prezime može imati najviše {RaftingUserEntity.Constraints.LastnameMaxLength} znakova.");

        RuleFor(x => x.Address)
            .MaximumLength(RaftingUserEntity.Constraints.AddressMaxLength)
            .WithMessage($"Adresa može imati najviše {RaftingUserEntity.Constraints.AddressMaxLength} znakova.");

        RuleFor(x => x.City)
            .MaximumLength(RaftingUserEntity.Constraints.CityMaxLength)
            .WithMessage($"Grad može imati najviše {RaftingUserEntity.Constraints.CityMaxLength} znakova.");
    }
}
