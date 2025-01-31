using Microsoft.AspNetCore.Mvc;
using CountriesApi.Application.Interfaces;
using CountriesApi.Domain.ResponseObjects.DTOs;

namespace InvesmentsApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestmentController : ControllerBase
    {
        private readonly IInvestmentService _invesmentService;

        public InvestmentController(IInvestmentService invesmentService)
        {
            _invesmentService = invesmentService;
        }

        [HttpPost("Invest")]
        public async Task<IActionResult> UpdateAllInvesments([FromBody] InvestmentDto investment)
        {
            var invested = await _invesmentService.Invest(investment);
            if (invested != null)
            {
                return invested.Value ? Ok(invested) : BadRequest(invested);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, invested?.ErrorMessage ?? "Internal Server Error, please contact the support.");
        }
    }
}