namespace Rafting.Application.Modules.Auth.Queries.GetMe;

public sealed class GetMeQueryHandler(IAppDbContext ctx, IAppCurrentUser currentUser)
    : IRequestHandler<GetMeQuery, GetMeQueryDto>
{
    public async Task<GetMeQueryDto> Handle(GetMeQuery request, CancellationToken ct)
    {
        var userId = currentUser.UserId
            ?? throw new RaftingUnauthorizedException("Korisnik nije prijavljen.");

        var dto = await ctx.Users
            .AsNoTracking()
            .Where(x => x.Id == userId)
            .Select(x => new GetMeQueryDto
            {
                Id = x.Id,
                Email = x.Email,
                Firstname = x.Firstname,
                Lastname = x.Lastname,
                Address = x.Address,
                City = x.City,
                Phone = x.Phone,
                AgencyId = x.AgencyId,
                IsAdmin = x.IsAdmin,
                IsManager = x.IsManager,
                IsEmployee = x.IsEmployee
            })
            .FirstOrDefaultAsync(ct);

        return dto ?? throw new RaftingNotFoundException($"Korisnik (ID={userId}) nije pronađen.");
    }
}
