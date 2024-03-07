namespace TravelAPI.DTOs
{
    public class RoomDTO
    {
        public string Number { get; set; }
        public string Type { get; set; }
        public double BaseRate { get; set; }
        public double Taxes { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public bool Enabled { get; set; }
    }
}
