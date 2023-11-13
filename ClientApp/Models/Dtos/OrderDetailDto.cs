using System.ComponentModel.DataAnnotations.Schema;

namespace ClientApp.Models.Dtos
{
    public class OrderDetailDto
  {
    public int OrderDetailId { get; set; }
    public int OrderId { get; set; }
    public int PieId { get; set; }
    public int Amount { get; set; }
    public decimal Price { get; set; }
    [NotMapped]
    public PieDto Pie { get; set; } = default!;
    [NotMapped]
    public OrderDto Order { get; set; } = default!;
  }
}
