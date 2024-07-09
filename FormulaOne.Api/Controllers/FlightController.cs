using AutoMapper;
using FormulaOne.Api.Services;
using FormulaOne.DataService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FormulaOne.Api.Controllers
{
    public class FlightController : BaseController
    {
        private readonly IFlightService _flightService;
        public FlightController(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            IFlightService flightService)
            : base(unitOfWork, mapper)
        {
            _flightService = flightService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFlights()
        {
            try
            {
                var result = await _flightService.GetAllAvailableFlights();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
