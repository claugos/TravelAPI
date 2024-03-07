using TravelAPI.DTOs;
using TravelAPI.Models;

namespace TravelAPI.Repositories
{
    public interface IReservationCollection
    {
        Task<List<ReservationDTO>> GetReservations();
        Task InsertReservation(Reservation reservation);
    }
}
