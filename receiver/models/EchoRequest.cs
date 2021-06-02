using System.ComponentModel.DataAnnotations;

namespace receiver
{
  public class EchoRequest
  {
    [Required(ErrorMessage = "Message is required")]
    public string Message { get; set; }
  }
}
