using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace receiver.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class EchoController : ControllerBase
  {
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public EchoResponse Post(EchoRequest req)
    {
      EchoResponse resp = new EchoResponse();
      resp.Message = $"The echo chamber says '{req.Message}'";
      return resp;
    }
  }
}
