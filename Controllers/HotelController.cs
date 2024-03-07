using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Net;
using TravelAPI.Models;
using TravelAPI.Repositories;
using TravelAPI.Services;

namespace TravelAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HotelController : ControllerBase
    {
        private readonly ILogger<HotelController> _logger;
        private readonly IHotelCollection _collection;
        private readonly IConfiguration _config;

        public HotelController(ILogger<HotelController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _collection = new HotelCollection(config);
        }

        [HttpPost("InsertHotel")]
        public async Task<IActionResult> InsertHotel([FromBody] Hotel hotel, [FromHeader] string token)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                JWTService jWT = new(_config);
                if (jWT.ValidateToken(token))
                {
                    foreach (Room room in hotel.Rooms)
                    {
                        room.DatabaseId = ObjectId.GenerateNewId();
                    }
                    await _collection.InsertHotel(hotel);
                    return Ok(new
                    {
                        code = HttpStatusCode.OK,
                        message = "Hotel inserted successfully"
                    });
                }
                return Unauthorized();
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while inserting the hotel: {ErrorMessage}", e.Message);
                return BadRequest(new
                {
                    code = HttpStatusCode.BadRequest,
                    message = "An error occurred while inserting the hotel."
                });
            }
        }

        [HttpPost("{hotelId}/RoomAssignment")]
        public async Task<IActionResult> RoomAssignment([FromBody] Room room, [FromRoute] string hotelId, [FromHeader] string token)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                JWTService jWT = new(_config);
                if (jWT.ValidateToken(token))
                {
                    ObjectId objectId = ObjectId.Parse(hotelId);
                    await _collection.AssignRoomToTheHotel(objectId, room);

                    return Ok(new
                    {
                        code = HttpStatusCode.OK,
                        message = "Room added to hotel successfully."
                    });
                }
                return Unauthorized();

            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while adding room to hotel:  {ErrorMessage}", e.Message);
                return BadRequest(new
                {
                    code = HttpStatusCode.BadRequest,
                    message = "An error occurred while adding room to hotel."
                });
            }
        }

        [HttpPut("{hotelId}/UpdateRoom")]
        public async Task<IActionResult> UpdateRoom([FromBody] Room room, [FromRoute] string hotelId, [FromHeader] string token)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                JWTService jWT = new(_config);
                if (jWT.ValidateToken(token))
                {
                    ObjectId objectId = ObjectId.Parse(hotelId);
                    await _collection.UpdateRoom(objectId, room);

                    return Ok(new
                    {
                        code = HttpStatusCode.OK,
                        message = "Room updated successfully."
                    });
                }
                return Unauthorized();

            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while updating the room:  {ErrorMessage}", e.Message);
                return BadRequest(new
                {
                    code = HttpStatusCode.BadRequest,
                    message = "An error occurred while updating the room"
                });
            }
        }

        [HttpPut("{hotelId}/UpdateRoomStatus")]
        public async Task<IActionResult> UpdateRoomStatus([FromQuery] string roomId, [FromRoute] string hotelId, [FromQuery] bool status, [FromHeader] string token)
        {
            try
            {
                JWTService jWT = new(_config);
                if (jWT.ValidateToken(token))
                {
                    ObjectId hotelObjectId = ObjectId.Parse(hotelId);
                    ObjectId roomObjectId = ObjectId.Parse(roomId);
                    await _collection.UpdateRoomStatus(hotelObjectId, roomObjectId, status);
                    return Ok(new
                    {
                        code = HttpStatusCode.OK,
                        message = "Room state updated successfully."
                    });
                }
                return Unauthorized();
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while updating the room state:  {ErrorMessage}", e.Message);
                return BadRequest(new
                {
                    code = HttpStatusCode.BadRequest,
                    message = "An error occurred while updating the room state."
                });
            }
        }

        [HttpPut("UpdateHotel")]
        public async Task<IActionResult> UpdateHotel([FromBody] Hotel hotel, [FromHeader] string token)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                JWTService jWT = new(_config);
                if (jWT.ValidateToken(token))
                {
                    await _collection.UpdateHotel(hotel);

                    return Ok(new
                    {
                        code = HttpStatusCode.OK,
                        message = "Hotel updated successfully."
                    });
                }
                return Unauthorized();
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while updating the hotel:  {ErrorMessage}", e.Message);
                return BadRequest(new
                {
                    code = HttpStatusCode.BadRequest,
                    message = "An error occurred while updating the hotel"
                });
            }
        }

        [HttpPut("UpdateHotelStatus/{hotelId}")]
        public async Task<IActionResult> UpdateHotelStatus([FromRoute] string hotelId, [FromQuery] bool status, [FromHeader] string token)
        {
            try
            {
                JWTService jWT = new(_config);
                if (jWT.ValidateToken(token))
                {
                    ObjectId hotelObjectId = ObjectId.Parse(hotelId);
                    await _collection.UpdateHotelStatus(hotelObjectId, status);

                    return Ok(new
                    {
                        code = HttpStatusCode.OK,
                        message = "Hotel state updated successfully."
                    });
                }
                return Unauthorized();

            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while updating the hotel state:  {ErrorMessage}", e.Message);
                return BadRequest(new
                {
                    code = HttpStatusCode.BadRequest,
                    message = "An error occurred while updating the hotel state."
                });
            }
        }

        [HttpGet("GetAvailableRooms")]
        public async Task<IActionResult> GetAvailableRooms([FromQuery] DateTime checkInDate, [FromQuery] DateTime checkOutDate, [FromQuery] int numberOfGuests, [FromQuery] string city)
        {
            try
            {
                if (checkInDate == default)
                {
                    return BadRequest(new
                    {
                        code = HttpStatusCode.BadRequest,
                        message = "The checkInDate field is required."
                    });
                }

                if (checkOutDate == default)
                {
                    return BadRequest(new
                    {
                        code = HttpStatusCode.BadRequest,
                        message = "The checkOutDate field is required."
                    });
                }

                if (numberOfGuests <= 0)
                {
                    return BadRequest(new
                    {
                        code = HttpStatusCode.BadRequest,
                        message = "The Guest number must be a positive value."
                    });
                }

                checkInDate = checkInDate.Date;
                checkOutDate = checkOutDate.Date.AddDays(1).AddTicks(-1);
                List<Hotel> rooms = await _collection.GetRooms(checkInDate, checkOutDate, numberOfGuests, city);
                return Ok(rooms);
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while updating the room state:  {ErrorMessage}", e.Message);
                return BadRequest(new
                {
                    code = HttpStatusCode.BadRequest,
                    message = "An error occurred while updating the room state."
                });
            }
        }
    }
}
