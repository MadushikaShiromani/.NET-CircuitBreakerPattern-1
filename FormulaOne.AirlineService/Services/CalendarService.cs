using FormulaOne.Entities.Dtos.Responses;

namespace FormulaOne.AirlineService.Services
{
    public class CalendarService : ICalendarService
    {
        private DateTime _recoveryTime = DateTime.UtcNow;
        private static readonly Random Random = new();
        private readonly ILogger<CalendarService> _logger;

        public CalendarService(ILogger<CalendarService> logger)
        {
            _logger = logger;
        }

        public Task<List<FlightDto>> GetAvailableFlights()
        {
            _logger.LogInformation($"Recovery time: {_recoveryTime}");
            _logger.LogInformation($"Date time: {DateTime.UtcNow}");

            

            if (_recoveryTime < DateTime.UtcNow && Random.Next(1, 4) == 1)
            {
                _logger.LogInformation($"Old recovery time: {_recoveryTime}");
                _recoveryTime = DateTime.UtcNow.AddSeconds(60);
                _logger.LogInformation($"New recovery time: {_recoveryTime}");

            }

            if (_recoveryTime > DateTime.UtcNow)
            {
                throw new Exception("Service not available");
            }

            var flights = new List<FlightDto>
            {
                new ()
                {
                    Arrival = "London",
                    Departure = "Dubai",
                    Price = 10000,
                    FlightDate = DateTime.Now.AddDays(3)
                },
                new ()
                {
                    Arrival = "Monaco",
                    Departure = "Miam",
                    Price = 14000,
                    FlightDate = DateTime.Now.AddDays(3)
                },
                new ()
                {
                    Arrival = "Madrid",
                    Departure = "Singapore",
                    Price = 9000,
                    FlightDate = DateTime.Now.AddDays(3)
                },
                new ()
                {
                    Arrival = "Athens",
                    Departure = "Tokyo",
                    Price = 18000,
                    FlightDate = DateTime.Now.AddDays(3)
                },
                new ()
                {
                    Arrival = "New York",
                    Departure = "Porto",
                    Price = 6000,
                    FlightDate = DateTime.Now.AddDays(3)
                }
            };
            return Task.FromResult(flights);
        }
    }
}
