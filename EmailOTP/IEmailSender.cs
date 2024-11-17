using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailOTP
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(string emailAddress, string emailBody);
    }
}
