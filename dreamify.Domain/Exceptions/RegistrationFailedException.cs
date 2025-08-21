namespace dreamify.Domain.Exceptions;

public class RegistrationFailedException(IEnumerable<string> errorDescpations) 
    : Exception($"Registration failed with following errors{Environment.NewLine}{string.Join(Environment.NewLine, errorDescpations)}");