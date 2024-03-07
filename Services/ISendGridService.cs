using SendGrid.Helpers.Mail;
using TravelAPI.Models;

namespace TravelAPI.Services
{
    public interface ISendGridService
    {
        Task SendSimpleMessage(List<EmailAddress> to, Reservation reservation, string roomType);
    }
}
