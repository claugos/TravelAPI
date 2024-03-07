using SendGrid.Helpers.Mail;
using SendGrid;
using TravelAPI.Models;

namespace TravelAPI.Services
{
    public class SendGridService : ISendGridService
    {

        private readonly IConfiguration _config;

        public SendGridService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendSimpleMessage(List<EmailAddress> to, Reservation reservation, string roomType)
        {
            var apiKey = _config.GetSection("SendGridKey").Value;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("claudiaisabelg08@gmail.com");
            var subject = "Confirmación de Reserva";
            var plainTextContent = "";
            var htmlContent = @"<!DOCTYPE html>
                                <html>
                                <head>
                                    <meta charset=""UTF-8"">
                                    <title>Confirmación de reserva</title>
                                    <style>
                                        body {
                                            font-family: Arial, sans-serif;
                                            background-color: #f5f5f5;
                                            color: #333;
                                        }
        
                                        .container {
                                            max-width: 600px;
                                            margin: 0 auto;
                                            background-color: #fff;
                                            padding: 20px;
                                            border-radius: 5px;
                                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                                        }
        
                                        h1 {
                                            color: #007bff;
                                        }
                                    </style>
                                </head>
                                <body>
                                    <div class=""container"">
                                        <h1>¡Reserva confirmada!</h1>
                                        <p>Estimado cliente,</p>
                                        <p>Nos complace informarle que su reserva de habitación ha sido confirmada. A continuación, encontrará los detalles de su reserva:</p>
        
                                        <h3>Detalles de la reserva</h3>
                                        <ul>" +
                                           $" <li><strong>Fecha de llegada:</strong> {reservation.CheckInDate.Date}</li>" +
                                           $" <li><strong>Fecha de salida:</strong> {reservation.CheckOutDate.Date}</li>" +
                                          $"  <li><strong>Tipo de habitación:</strong> {roomType}</li>" +
                                          $"  <li><strong>Número de huéspedes:</strong> {reservation.NumberOfGuests}</li>" +
                                       @" </ul>
        
                                        <p>Gracias por elegirnos para su estancia. Esperamos brindarle una experiencia inolvidable.</p>
        
                                    </div>
                                </body>
                                </html>";
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, to, subject, plainTextContent, htmlContent);
            await client.SendEmailAsync(msg);
        }
    }
}
