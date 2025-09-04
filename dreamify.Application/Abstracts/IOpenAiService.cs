using dreamify.Domain.Requests;
using dreamify.Domain.Response;

namespace dreamify.Application.Abstracts;

public interface IOpenAiService
{
    public Task<AnalysisResponse> AnayzeDream(AnalysisRequest request);

}