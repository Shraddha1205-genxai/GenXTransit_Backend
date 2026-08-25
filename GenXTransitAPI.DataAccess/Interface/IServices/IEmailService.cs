using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(
             string to,
             string subject,
             string body);

        Task SendUserCreatedEmail(
            string toEmail,
            string loginId,
            string password);
    }
}
