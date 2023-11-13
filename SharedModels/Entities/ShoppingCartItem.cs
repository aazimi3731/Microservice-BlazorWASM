using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SharedModels.Entities
{
  public class ShoppingCartItem
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ShoppingCartItemId { get; set; }
    public int PieId { get; set; }
    public int Amount { get; set; }
    public decimal Price { get; set; }
    public string? ShoppingCartId { get; set; }
    public bool IsOrdered { get; set; }
  }
}
