using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EmailOTP
{
    public class MockEmailSender:IEmailSender
    {
        public  Task<bool> SendEmailAsync(string emailAddress, string emailBody)
        {
           Console.WriteLine($"Email sent to {emailAddress} with body: {emailBody}");
            return Task.FromResult(true);
        }
    }
}
