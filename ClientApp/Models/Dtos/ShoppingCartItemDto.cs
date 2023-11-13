using System.ComponentModel.DataAnnotations.Schema;

namespace ClientApp.Models.Dtos
{
    public class ShoppingCartItemDto
  {
    public int ShoppingCartItemId { get; set; }
    public int PieId { get; set; }
    [NotMapped]
    public PieDto Pie { get; set; } = default!;
    public int Amount { get; set; }
    public decimal Price { get; set; }
    public string? ShoppingCartId { get; set; }
    public bool IsOrdered { get; set; }
  }
}
