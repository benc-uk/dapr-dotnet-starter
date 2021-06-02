using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace receiver
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
          webBuilder
          .UseStartup<Startup>()
          // Force plain HTTP connection, and bind only to localhost
          .UseUrls("http://127.0.0.1:5000");
        });
  }
}
