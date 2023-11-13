using ClientApp.Models.Dtos;

namespace ClientApp.Interfaces
{
    public interface ICategoryService
  {
    Task<IEnumerable<CategoryDto>> AllCategories();
  }
}
