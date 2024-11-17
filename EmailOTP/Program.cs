// See https://aka.ms/new-console-template for more information
using EmailOTP;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Setup DI container
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Resolve the EmailOtpModule
        var emailOtpModule = serviceProvider.GetRequiredService<EmailOTPModule>();

        // Start the OTP module
        emailOtpModule.Start();

        Console.WriteLine("Enter your email:");
        string userEmail = Console.ReadLine();

        // Generate and send OTP
        var emailStatus = await emailOtpModule.GenerateOtpEmailAsync(userEmail);
        Console.WriteLine($"Email Status: {emailStatus}");

        if (emailStatus == EmailOTPModule.STATUS_EMAIL_OKAY)
        {
            //Check otp
            var checkStatus = await emailOtpModule.CheckOtpAsync();
            Console.WriteLine($"OTP Check Status: {checkStatus}");
        }
        emailOtpModule.Close();
        }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IEmailSender, MockEmailSender>();
        services.AddTransient<EmailOTPModule>();
    }
}