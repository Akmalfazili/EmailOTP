# EmailOTP
 Email OTP Coding Assessment

# Assumptions
1. Utilized TOTP (Time-based one-time password) package to generate OTP for 1 minute with maximum of 10 tries.
2. The project uses a mock email sender (MockEmailSender) for testing purposes.
3. For production, the mock can be replaced with a real email-sending service (SMTP).
4. A random 20-byte secret key is generated and stored in memory for the duration of the module's lifecycle.
5. The same key is used for both generating and verifying OTPs.
6. Users must provide an email address ending with '@dso.org.sg'.
7. Assume that all valid email addresses provided belong to users authorized to use the service

# How to test module
1. Run the program
2. Provide a valid email (e.g., user@dso.org.sg) to receive a mock OTP.
3. Check the console for the OTP code sent via the mock email sender.
4. Enter the OTP code to validate it.
5. Test cases like entering the wrong OTP, waiting for the code to expire, or exceeding the maximum attempts and invalid email.
