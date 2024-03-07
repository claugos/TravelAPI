using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using SendGrid.Helpers.Mail;
using System.Net;
using TravelAPI.DTOs;
using TravelAPI.Models;
using TravelAPI.Repositories;
using TravelAPI.Services;

namespace TravelAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ILogger<ReservationController> _logger;
        private readonly IReservationCollection _collection;
        private readonly IHotelCollection _collectionHotel;
        private readonly ISendGridService _sendGridService;
        private readonly IConfiguration _config;

        public ReservationController(ILogger<ReservationController> logger, ISendGridService sendGridService, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _sendGridService = sendGridService;
            _collection = new ReservationCollection(config);
            _collectionHotel = new HotelCollection(config);
        }

        [HttpPost("InsertReservation")]
        public async Task<IActionResult> InsertHotel([FromBody] Reservation reservation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                reservation.CheckInDate = reservation.CheckInDate.Date;
                reservation.CheckOutDate = reservation.CheckOutDate.Date.AddDays(1).AddTicks(-1);
                await _collection.InsertReservation(reservation);
                List<string> to = reservation.Guests.Select(x => x.Email).ToList();
                List<EmailAddress> emailAddresses = to.Select(correo => new EmailAddress(correo)).ToList();
                Room room = await _collectionHotel.GetRoomById(ObjectId.Parse(reservation.IdRoom), ObjectId.Parse(reservation.IdHotel));
                await _sendGridService.SendSimpleMessage(emailAddresses, reservation, room.Type);
                return Ok(new
                {
                    code = HttpStatusCode.OK,
                    message = "Reservation inserted successfully"
                });
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while inserting the reservation:  {ErrorMessage}", e.Message);
                return BadRequest(new
                {
                    code = HttpStatusCode.BadRequest,
                    message = "An error occurred while inserting the reservation."
                });
            }
        }

        [HttpGet("GetReservations")]
        public async Task<IActionResult> GetReservations([FromHeader] string token)
        {
            try
            {
                JWTService jWT = new(_config);
                if (jWT.ValidateToken(token))
                {
                    List<ReservationDTO> response = await _collection.GetReservations();
                    return Ok(response);
                }
                return Unauthorized();
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while getting the reservations:  {ErrorMessage}", e.Message);
                return BadRequest(new
                {
                    code = HttpStatusCode.BadRequest,
                    message = "An error occurred while getting the reservations."
                });
            }
        }
    }
}
