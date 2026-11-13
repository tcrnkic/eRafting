namespace Market.Application.Abstractions;

/// <summary>
/// Represents the currently logged-in user in the system.
/// </summary>
public interface IAppCurrentUser
{
    /// <summary>
    /// User identifier (UserId).
    /// </summary>
    int? UserId { get; }

    /// <summary>
    /// User Email. (optional)
    /// </summary>
    string? Email { get; }

    /// <summary>
    /// Indicates whether the user is logged in.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Indicates whether the user is an administrator.
    /// </summary>
    bool IsAdmin { get; }

    /// <summary>
    /// Indicates whether the user is a manager.
    /// </summary>
    bool IsManager { get; }

    /// <summary>
    /// Indicates whether the user is a regular employee.
    /// </summary>
    bool IsEmployee { get; }
    /// <summary>
    /// Agency of the logged-in owner or employee (from the "agency_id" claim).
    /// Null for platform admins and guests.
    /// </summary>
    int? AgencyId { get; }
}