using MongoDB.Bson;
using MongoDB.Driver;
using TravelAPI.DTOs;
using TravelAPI.Models;

namespace TravelAPI.Repositories
{
    public class ReservationCollection : IReservationCollection
    {
        private readonly IMongoCollection<Reservation> collection;

        public ReservationCollection(IConfiguration config)
        {
            collection = MongoDBRepository.GetInstance(config).db.GetCollection<Reservation>("Reservations");
        }

        public async Task<List<ReservationDTO>> GetReservations()
        {
            var pipeline = new[]
             {
                new BsonDocument("$lookup", new BsonDocument
                {
                    {"from", "Hotels"},
                    {"localField", "HotelId"},
                    {"foreignField", "_id"},
                    {"as", "hotel"}
                }),
                new BsonDocument("$unwind", new BsonDocument("path", "$hotel")),
                new BsonDocument("$unwind", new BsonDocument("path", "$hotel.Rooms")),
                new BsonDocument("$redact", new BsonDocument
                {
                    {"$cond", new BsonDocument
                    {
                        {"if", new BsonDocument("$eq", new BsonArray { "$RoomId", "$hotel.Rooms._id" })},
                        {"then", "$$KEEP"},
                        {"else", "$$PRUNE"}
                    }}
                }),
                new BsonDocument("$project", new BsonDocument
                {
                    {"_id", 0},
                    {"DatabaseId", "$_id"},
                    {"CheckInDate", "$CheckInDate"},
                    {"CheckOutDate", "$CheckOutDate"},
                    {"NumberOfGuests", "$NumberOfGuests"},
                    {"Hotel", new BsonDocument
                    {
                        {"Name", "$hotel.Name"},
                        {"Address", "$hotel.Address"},
                        {"Enabled", "$hotel.Enabled"}
                    }},
                    {"Room", new BsonDocument
                    {
                        {"Number", "$hotel.Rooms.Number"},
                        {"Type", "$hotel.Rooms.Type"},
                        {"BaseRate", "$hotel.Rooms.BaseRate"},
                        {"Taxes", "$hotel.Rooms.Taxes"},
                        {"Location", "$hotel.Rooms.Location"},
                        {"Capacity", "$hotel.Rooms.Capacity"},
                        {"Enabled", "$hotel.Rooms.Enabled"}
                    }},
                    {"Guests", "$Guests"},
                    {"EmergencyContact", "$EmergencyContact"}
                })
            };

            List<ReservationDTO> reservations = await collection.Aggregate<ReservationDTO>(pipeline).ToListAsync();
            return reservations;
        }

        public async Task InsertReservation(Reservation reservation)
        {
            await collection.InsertOneAsync(reservation);
        }
    }
}
