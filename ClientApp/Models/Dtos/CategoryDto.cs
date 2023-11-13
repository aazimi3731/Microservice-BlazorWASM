using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientApp.Models.Dtos
{
  public class CategoryDto
  {
    [Key]
    [Column("CategoryDtoId")]
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public List<PieDto>? Pies { get; set; }
  }
}
