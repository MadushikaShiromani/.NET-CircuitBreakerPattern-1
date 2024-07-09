using FormulaOne.Entities.Dtos.Responses;

namespace FormulaOne.Api.Services
{
    public interface IFlightService
    {
        Task<List<FlightDto>> GetAllAvailableFlights();
    }
}
