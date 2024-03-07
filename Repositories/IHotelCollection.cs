using MongoDB.Bson;
using TravelAPI.Models;

namespace TravelAPI.Repositories
{
    public interface IHotelCollection
    {
        Task AssignRoomToTheHotel(ObjectId hotelId, Room room);
        Task<List<Hotel>> GetRooms(DateTime checkInDate, DateTime checkOutDate, int numberOfGuests, string city);
        Task<Room> GetRoomById(ObjectId roomId, ObjectId hotelId);
        Task InsertHotel(Hotel hotel);
        Task UpdateHotel(Hotel hotel);
        Task UpdateHotelStatus(ObjectId hotelId, bool status);
        Task UpdateRoom(ObjectId hotelId, Room room);
        Task UpdateRoomStatus(ObjectId hotelId, ObjectId roomId, bool status);
    }
}
