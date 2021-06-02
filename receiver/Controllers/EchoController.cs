using System;
using System.Net.Http;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace receiver.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class EchoController : ControllerBase
  {
    private static readonly string storeName = "statestore";
    private readonly string daprPort = "3500";
    static readonly HttpClient client = new();

    public EchoController()
    {
      // Locate Dapr sidecar/process port
      this.daprPort = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT");
      if (this.daprPort == null || this.daprPort.Length == 0)
      {
        this.daprPort = "3500";
      }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    async public Task<EchoResponse> Post(EchoRequest req)
    {
      string daprStateUrl = $"http://localhost:{this.daprPort}/v1.0/state/{storeName}";
      Console.WriteLine($"### Making call to Dapr API: {daprStateUrl}");

      // A very quick and dirty state request payload, must be an array
      // See: https://docs.dapr.io/reference/api/state_api/#request-body 
      var state = new[] {
        new {
          // There could be better keys to use
          key = HttpContext.Connection.RemoteIpAddress.ToString(),
          value = req.Message
        },
      };

      // Build and send HTTP POST request to Dapr state API
      try
      {
        JsonContent payload = JsonContent.Create(state);
        HttpResponseMessage stateResp = await client.PostAsync(daprStateUrl, payload);
        if (stateResp.StatusCode == HttpStatusCode.NoContent)
        {
          Console.WriteLine("### OK: Message was stored into Dapr state successfully");
        }
        else
        {
          Console.WriteLine($"### ERROR: Failed to store message in Dapr state: {stateResp.StatusCode}");
        }

      }
      catch (Exception e)
      {
        Console.WriteLine($"### ERROR: Call to Dapr state API failed! {e.Message}");
      }

      // Respond back to client with simple echo of input message
      EchoResponse resp = new()
      {
        Message = $"The echo chamber says '{req.Message}'"
      };
      return resp;
    }
  }
}
