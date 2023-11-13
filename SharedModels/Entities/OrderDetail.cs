using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SharedModels.Entities
{
  public class OrderDetail
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderDetailId { get; set; }
    public int OrderId { get; set; }
    public int PieId { get; set; }
    public int Amount { get; set; }
    public decimal Price { get; set; }
    public Order Order { get; set; } = default!;
  }
}
