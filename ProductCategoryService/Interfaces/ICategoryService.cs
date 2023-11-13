using SharedModels.Entities;

namespace ProductCategoryService.Models.Interfaces
{
  public interface ICategoryService
  {
    #region Properties

    Task<IEnumerable<Category>> AllCategories();

    #endregion
  }
}
