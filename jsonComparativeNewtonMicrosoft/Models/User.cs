namespace jsonComparativeNewtonMicrosoft.Models;

internal record User(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email
);