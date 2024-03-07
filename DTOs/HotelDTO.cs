using TravelAPI.Models;

namespace TravelAPI.DTOs
{
    public class HotelDTO
    {
        public string Name { get; set; }
        public Address Address { get; set; }
        public bool Enabled { get; set; }
    }
}
