using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace TravelAPI.DTOs
{
    public class ReservationDTO
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public ObjectId DatabaseId { get; set; }

        [BsonIgnore]
        public string Id
        {
            get => DatabaseId.ToString();
            set => DatabaseId = ObjectId.Parse(value);
        }

        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public HotelDTO Hotel { get; set; }
        public RoomDTO Room { get; set; }
        public List<GuestDTO> Guests { get; set; }
        public EmergencyContactDTO EmergencyContact { get; set; }
    }
}
