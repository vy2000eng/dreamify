namespace dreamify.Domain.Exceptions;

public class UserAlreadyExistsException(string email) : Exception($"User With Email {email} already exists.");
