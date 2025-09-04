namespace dreamify.Domain.Exceptions;

public class OpenAiAnalysisFiled(HttpRequestError requestError, string requestErrorMessage) : Exception($"request failed with: {requestError} and {requestErrorMessage}");
