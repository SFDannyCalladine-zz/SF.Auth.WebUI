using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;

namespace SF.Auth.WebUI
{
    public class Program
    {
        #region Public Methods

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Any, 5000);

                    options.Listen(IPAddress.Any, 44329, listenOptions =>
                    {
                        //listenOptions.UseHttps("certificate.pfx", "BlinkyPlant18");
                        listenOptions.UseHttps(new HttpsConnectionAdapterOptions
                        {
                            ServerCertificate = new X509Certificate2("certificate.pfx", "BlinkyPlant18"),
                            ClientCertificateMode = ClientCertificateMode.NoCertificate
                        });
                    });
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        #endregion Public Methods
    }
}