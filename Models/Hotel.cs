using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TravelAPI.Models
{
    public class Hotel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [System.Text.Json.Serialization.JsonIgnore]
        public ObjectId DatabaseId { get; set; }

        [BsonIgnore]
        public string Id
        {
            get => DatabaseId.ToString();
            set => DatabaseId = ObjectId.Parse(value);
        }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The hotel name must have between 3 and 100 characters.")]
        public string Name { get; set; }

        [Required]
        public Address Address { get; set; }
        public bool Enabled { get; set; }
        public List<Room> Rooms { get; set; }
    }

    public class Address
    {
        [Required]
        [StringLength(100, ErrorMessage = "The city cannot have more than 100 characters.")]
        public string City { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The country cannot have more than 100 characters.")]
        public string Country { get; set; }
    }

    public class Room
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [System.Text.Json.Serialization.JsonIgnore]
        public ObjectId DatabaseId { get; set; }

        [BsonIgnore]
        public string Id
        {
            get => DatabaseId.ToString();
            set => DatabaseId = ObjectId.Parse(value);
        }

        [Required(ErrorMessage = "The room number is required.")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "The room number must be between 2 and 10 characters.")]
        public string Number { get; set; }

        [Required(ErrorMessage = "The room type is required.")]
        [StringLength(50, ErrorMessage = "The room type cannot be more than 50 characters.")]
        public string Type { get; set; }

        [Required(ErrorMessage = "The base rate is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "The base rate must be a positive value.")]
        public double BaseRate { get; set; }

        [Required(ErrorMessage = "The taxes are required.")]
        [Range(0, double.MaxValue, ErrorMessage = "The taxes must be a positive value.")]
        public double Taxes { get; set; }

        [Required(ErrorMessage = "The room location is required.")]
        [StringLength(100, ErrorMessage = "The room location cannot be more than 100 characters.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "The room capacity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "The capacity must be a positive value.")]
        public int Capacity { get; set; }

        public bool Enabled { get; set; }
    }
}
