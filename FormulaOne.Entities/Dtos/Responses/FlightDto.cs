namespace FormulaOne.Entities.Dtos.Responses
{
    public class FlightDto
    {
        public string Arrival { get; set; } = string.Empty;
        public string? Departure { get; set; } = string.Empty;
        public int Price { get; set; }
        public DateTime FlightDate { get; set; }
    }
}
