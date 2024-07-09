using FormulaOne.Entities.Dtos.Responses;

namespace FormulaOne.AirlineService.Services
{
    public interface ICalendarService
    {
        Task<List<FlightDto>> GetAvailableFlights();
    }
}
