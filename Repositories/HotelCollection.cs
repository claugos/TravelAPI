using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using TravelAPI.Models;

namespace TravelAPI.Repositories
{
    public class HotelCollection : IHotelCollection
    {
        private readonly IMongoCollection<Hotel> collection;
        private readonly IConfiguration _config;

        public HotelCollection(IConfiguration config)
        {
            _config = config;
            collection = MongoDBRepository.GetInstance(config).db.GetCollection<Hotel>("Hotels");
        }

        public async Task AssignRoomToTheHotel(ObjectId hotelId, Room room)
        {
            room.DatabaseId = ObjectId.GenerateNewId();
            var filter = Builders<Hotel>.Filter.Eq(h => h.DatabaseId, hotelId);
            var update = Builders<Hotel>.Update.Push(h => h.Rooms, room);

            await collection.UpdateOneAsync(filter, update);
        }

        public async Task<Room> GetRoomById(ObjectId roomId, ObjectId hotelId)
        {
            var filter = Builders<Hotel>.Filter.And(
                Builders<Hotel>.Filter.Eq(h => h.DatabaseId, hotelId),
                Builders<Hotel>.Filter.ElemMatch(x => x.Rooms, Builders<Room>.Filter.Eq(h => h.DatabaseId, roomId))
            );

            var projection = Builders<Hotel>.Projection.ElemMatch(x => x.Rooms, Builders<Room>.Filter.Eq(h => h.DatabaseId, roomId));

            var result = await collection.Find(filter).Project(projection).FirstOrDefaultAsync();
            if (result != null && result.Contains("Rooms"))
            {
                var roomBson = result["Rooms"].AsBsonArray[0].AsBsonDocument;
                var room = BsonSerializer.Deserialize<Room>(roomBson);
                return room;
            }
            return new Room();
        }

        public async Task<List<Hotel>> GetRooms(DateTime checkInDate, DateTime checkOutDate, int numberOfGuests, string city)
        {
            var filter = Builders<Hotel>.Filter.And(
                Builders<Hotel>.Filter.ElemMatch(
                    hotel => hotel.Rooms,
                    roomFilter => roomFilter.Capacity >= numberOfGuests && roomFilter.Enabled == true
                ),
                Builders<Hotel>.Filter.Eq(hotel => hotel.Address.City, city),
                Builders<Hotel>.Filter.Eq(hotel => hotel.Enabled, true)
            );

            var reservationsCollection = MongoDBRepository.GetInstance(_config).db.GetCollection<Reservation>("Reservations");

            //var unavailableRoomsFilter = Builders<Reservation>.Filter.And(
            //    Builders<Reservation>.Filter.Gte(r => r.CheckInDate, checkInDate),
            //    Builders<Reservation>.Filter.Lte(r => r.CheckOutDate, checkOutDate)
            //); 
            var unavailableRoomsFilter = Builders<Reservation>.Filter.Or(
                Builders<Reservation>.Filter.And(Builders<Reservation>.Filter.Gte(r => r.CheckInDate, checkInDate),
               Builders<Reservation>.Filter.Lte(r => r.CheckInDate, checkOutDate)),
                Builders<Reservation>.Filter.And(Builders<Reservation>.Filter.Gte(r => r.CheckOutDate, checkInDate),
               Builders<Reservation>.Filter.Lte(r => r.CheckOutDate, checkOutDate))
            );

            var unavailableRoomIds = (await reservationsCollection.Find(unavailableRoomsFilter)
                .Project(r => r.RoomId)
                .ToListAsync())
                .Select(r => r);

            filter &= Builders<Hotel>.Filter.ElemMatch(
                hotel => hotel.Rooms,
                room => room.Enabled && !unavailableRoomIds.Contains(room.DatabaseId)
            );

            var hotels = await collection.Find(filter).ToListAsync();

            hotels.ForEach(hotel =>
            {
                hotel.Rooms = hotel.Rooms.Where(room => room.Enabled && !unavailableRoomIds.Contains(room.DatabaseId) && room.Capacity >= numberOfGuests && room.Enabled == true).ToList();
            });

            return hotels;
        }

        public async Task InsertHotel(Hotel hotel)
        {
            await collection.InsertOneAsync(hotel);
        }

        public async Task UpdateHotel(Hotel hotel)
        {
            var filter = Builders<Hotel>.Filter.Eq(h => h.DatabaseId, hotel.DatabaseId);
            var update = Builders<Hotel>.Update
                .Set(h => h.Name, hotel.Name)
                .Set(h => h.Address, hotel.Address);

            await collection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateHotelStatus(ObjectId hotelId, bool status)
        {
            var filter = Builders<Hotel>.Filter.Eq(h => h.DatabaseId, hotelId);
            var update = Builders<Hotel>.Update
                .Set(h => h.Enabled, status);

            await collection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateRoom(ObjectId hotelId, Room room)
        {
            room.DatabaseId = ObjectId.Parse(room.Id);
            var filter = Builders<Hotel>.Filter.Eq(h => h.DatabaseId, hotelId);
            var hotel = collection.Find(filter).FirstOrDefault();

            if (hotel != null)
            {
                var index = hotel.Rooms.FindIndex(r => r.DatabaseId == room.DatabaseId);

                if (index >= 0)
                {
                    filter = Builders<Hotel>.Filter
                        .And(Builders<Hotel>.Filter.Eq(h => h.DatabaseId, hotelId),
                         Builders<Hotel>.Filter.ElemMatch(h => h.Rooms, r => r.DatabaseId == room.DatabaseId));
                    var update = Builders<Hotel>.Update.Set($"Rooms.{index}", room);

                    await collection.UpdateOneAsync(filter, update);
                }
            }
        }

        public async Task UpdateRoomStatus(ObjectId hotelId, ObjectId roomId, bool status)
        {
            var filter = Builders<Hotel>.Filter.Eq(h => h.DatabaseId, hotelId);
            var hotel = collection.Find(filter).FirstOrDefault();

            if (hotel != null)
            {
                var index = hotel.Rooms.FindIndex(r => r.DatabaseId == roomId);

                if (index >= 0)
                {
                    filter = Builders<Hotel>.Filter
                        .And(Builders<Hotel>.Filter.Eq(h => h.DatabaseId, hotelId),
                         Builders<Hotel>.Filter.ElemMatch(h => h.Rooms, r => r.DatabaseId == roomId));
                    var update = Builders<Hotel>.Update.Set($"Rooms.{index}.Enabled", status);

                    await collection.UpdateOneAsync(filter, update);
                }
            }
        }
    }
}
