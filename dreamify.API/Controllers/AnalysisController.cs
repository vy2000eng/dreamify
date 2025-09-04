using dreamify.Application.Abstracts;
using dreamify.Application.Services;
using dreamify.Domain.Requests;
using dreamify.Domain.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace dreamify.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AnalysisController: ControllerBase
{
    private readonly IOpenAiService _openAiService;

    public AnalysisController(IOpenAiService openAiService)
    {
        _openAiService = openAiService;
    }
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("analyzeDream")]
    public async Task<IResult> AnalyzeDream(AnalysisRequest request)
    {
        
       
        var response = await  _openAiService.AnayzeDream(request);
       
    
        return Results.Ok(response);
    }


    
}