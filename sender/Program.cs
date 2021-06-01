using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace sender
{
  class Program
  {
    static readonly HttpClient client = new HttpClient();
    static string targetDaprApp = "receiver";
    static string targetDaprMethod = "echo";
    static int interval = 5000;

    static async Task Main(string[] args)
    {
      while (true) {
        await CallDaprReceiver();
        Thread.Sleep(interval);
      }
    }

    private static async Task CallDaprReceiver() {
      try	{
        // Make a GET call, this would more often be a POST
        string url = $"http://localhost:3500/v1.0/invoke/{targetDaprApp}/{targetDaprMethod}";
        Console.Write($"### Making call to: {url}");
        HttpResponseMessage resp = await client.GetAsync(url);
        string body = await resp.Content.ReadAsStringAsync();
        Console.Write($"### {resp.StatusCode} {body}");
      } catch(Exception e) {
        Console.WriteLine("\nException Caught!");	
        Console.WriteLine($"Message - {e.Message} ");
      }
      return;
    }

  }
}
