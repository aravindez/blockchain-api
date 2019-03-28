using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace blockchain_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSetting("http_port", "5000")
                .UseStartup<Startup>();
    }
}
