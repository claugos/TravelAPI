using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TravelAPI.Models
{
    public class Reservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public ObjectId DatabaseId { get; set; }

        [BsonIgnore]
        public string Id
        {
            get => DatabaseId.ToString();
            set => DatabaseId = ObjectId.Parse(value);
        }

        [Required(ErrorMessage = "Check-in date is required.")]
        public DateTime CheckInDate { get; set; }

        [Required(ErrorMessage = "Check-out date is required.")]
        public DateTime CheckOutDate { get; set; }

        [Required(ErrorMessage = "Number of guests is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of guests must be at least 1.")]
        public int NumberOfGuests { get; set; }

        [Required(ErrorMessage = "Hotel ID is required.")]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public ObjectId HotelId { get; set; }

        [BsonIgnore]
        public string IdHotel
        {
            get => HotelId.ToString();
            set => HotelId = ObjectId.Parse(value);
        }

        [Required(ErrorMessage = "Room ID is required.")]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public ObjectId RoomId { get; set; }

        [BsonIgnore]
        public string IdRoom
        {
            get => RoomId.ToString();
            set => RoomId = ObjectId.Parse(value);
        }

        [Required(ErrorMessage = "At least one guest is required.")]
        [MinLength(1, ErrorMessage = "At least one guest is required.")]
        public List<Guest> Guests { get; set; }

        [Required(ErrorMessage = "Emergency contact is required.")]
        public EmergencyContact EmergencyContact { get; set; }
    }

    public class Guest
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Birth date is required.")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(10, ErrorMessage = "Gender cannot be longer than 10 characters.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Document type is required.")]
        [StringLength(50, ErrorMessage = "Document type cannot be longer than 50 characters.")]
        public string DocumentType { get; set; }

        [Required(ErrorMessage = "Document number is required.")]
        [StringLength(20, ErrorMessage = "Document number cannot be longer than 20 characters.")]
        public string DocumentNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact phone is required.")]
        [StringLength(20, ErrorMessage = "Contact phone cannot be longer than 20 characters.")]
        public string ContactPhone { get; set; }
    }

    public class EmergencyContact
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Full name cannot be longer than 100 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Contact phone is required.")]
        [StringLength(20, ErrorMessage = "Contact phone cannot be longer than 20 characters.")]
        public string ContactPhone { get; set; }
    }
}
