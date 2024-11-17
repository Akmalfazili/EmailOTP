using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OtpNet;




namespace EmailOTP
{
    public class EmailOTPModule
    {
        public const string STATUS_EMAIL_OKAY = "STATUS_EMAIL_OKAY";
        public const string STATUS_EMAIL_FAIL = "STATUS_EMAIL_FAIL";
        public const string STATUS_EMAIL_INVALID = "STATUS_EMAIL_INVALID";

        public const string STATUS_OTP_OK = "STATUS_OTP_OK";
        public const string STATUS_OTP_FAIL = "STATUS_OTP_FAIL";
        public const string STATUS_OTP_TIMEOUT = "STATUS_OTP_TIMEOUT";

        private readonly IEmailSender _emailSender;
        private const int MaxAttempts = 10;
        private const int OtpValidityDurationInSeconds = 60; // OTP is valid for 60 seconds
        private DateTime _otpExpiry;
        private byte[] _secretKey;
        private string totpCode;

        public EmailOTPModule(IEmailSender emailSender)
        {
            _emailSender= emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _secretKey = GenerateSecretKey();

        }
        private byte [] GenerateSecretKey()
        {
            // Generate a random 20-byte secret key
            byte[] secretKey = new byte[20];
            new Random().NextBytes(secretKey);

            return secretKey;
        }

        public void Start()
        {
            Console.WriteLine("Email OTP module has started");
        }

        public void Close()
        {
            Console.WriteLine("Email OTP module is closed");
        }

        public async Task<string> GenerateOtpEmailAsync(string userEmail)
        {
            //validate email with dso.org.sg
            if (!userEmail.EndsWith("@dso.org.sg"))
            {
                return STATUS_EMAIL_INVALID;
            }

            //Generate otp with 1 minute validity and 6 digits using TOTP method
            var totp = new Totp(_secretKey, step: OtpValidityDurationInSeconds, totpSize: 6);
            DateTime timeNow = DateTime.UtcNow;
            var totpCode = totp.ComputeTotp(timeNow);
            _otpExpiry = timeNow.AddMinutes(1);

            //Email body
            var emailBody = $"Your OTP Code is {totpCode}. The code is valid for 1 minute.";

            // Print email status, email address and body to console for testing
            Console.WriteLine($"Email To: {userEmail}");
            Console.WriteLine($"Email Body: {emailBody}");

            //Send email
            bool emailSent = await _emailSender.SendEmailAsync(userEmail, emailBody);

            return emailSent ? STATUS_EMAIL_OKAY :  STATUS_EMAIL_FAIL;

        }

        public async Task<string> CheckOtpAsync()
        {
            //attempt counter
            int attempts = 0;
            
            Console.WriteLine("Enter your OTP:");

            while (attempts < MaxAttempts && DateTime.UtcNow < _otpExpiry)
            {
                if (DateTime.UtcNow > _otpExpiry)
                {
                    Console.WriteLine("OTP has expired.");
                    return STATUS_OTP_TIMEOUT;
                }
                string userOtp = Console.ReadLine();
                attempts++;
                // verify if the inputted otp by user is valid and within timeframe
                var totp = new Totp(_secretKey, step: OtpValidityDurationInSeconds, totpSize: 6);
                bool isValid = totp.VerifyTotp(userOtp, out long timeWindowUsed);
                if (isValid)
                {
                    return STATUS_OTP_OK;
                }

                // Provide feedback on how many attempts are left
                int attemptsLeft = MaxAttempts - attempts;
                Console.WriteLine($"Incorrect OTP. You have {attemptsLeft} attempts left.Try again:");
            }
            return STATUS_OTP_FAIL;
        }
    }
}
