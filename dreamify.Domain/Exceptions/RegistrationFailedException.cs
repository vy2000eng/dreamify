namespace dreamify.Domain.Exceptions;

public class RegistrationFailedException(IEnumerable<string> errorDescriptions) 
    : Exception($"Registration failed with following errors{Environment.NewLine}{string.Join(Environment.NewLine, errorDescriptions)}");