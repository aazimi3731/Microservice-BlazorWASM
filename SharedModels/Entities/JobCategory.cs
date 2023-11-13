using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SharedModels.Entities
{
  public class JobCategory
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int JobCategoryId { get; set; }
    public string? Name { get; set; } = string.Empty;
  }
}
