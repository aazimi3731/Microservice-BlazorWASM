using SharedModels.Entities;

namespace ProductCategoryService.Models.Interfaces
{
  public interface ICategoryRepository
  {
    #region Properties

    Task<IEnumerable<Category>> AllCategories();

    #endregion
  }
}
