using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http.Json;

namespace sender
{
  class Program
  {
    static readonly HttpClient client = new();
    static readonly string targetDaprApp = "receiver";
    static readonly string targetDaprMethod = "echo";
    static readonly int interval = 5000;

    static async Task Main()
    {
      // Locate Dapr sidecar/process port
      string daprPort = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT");
      if (daprPort == "")
      {
        daprPort = "3500";
      }

      // Infinite loop invoking Dapr receiver
      while (true)
      {
        await CallDaprReceiver(daprPort);
        Thread.Sleep(interval);
      }
    }

    private static async Task CallDaprReceiver(string daprPort)
    {
      try
      {
        string url = $"http://localhost:{daprPort}/v1.0/invoke/{targetDaprApp}/method/{targetDaprMethod}";
        Console.WriteLine($"### Making call to Dapr API: {url}");

        var echoRequest = new
        {
          message = "Hello from Dapr :)"
        };

        JsonContent payload = JsonContent.Create(echoRequest);
        HttpResponseMessage resp = await client.PostAsync(url, payload);
        string body = await resp.Content.ReadAsStringAsync();
        Console.WriteLine($"### {resp.StatusCode} {body}");
      }
      catch (Exception e)
      {
        Console.WriteLine("\nException Caught!");
        Console.WriteLine($"Message - {e.Message} ");
      }
      return;
    }

  }
}
