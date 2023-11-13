using System.ComponentModel.DataAnnotations.Schema;

namespace ClientApp.Models.Dtos
{
    public class ShoppingCartDto
  {
    public string? ShoppingCartId { get; set; }
    public string? UserId { get; set; }
  }
}
