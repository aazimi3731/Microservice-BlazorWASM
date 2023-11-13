using SharedModels.Entities;

namespace ProductCategoryService.Models.Interfaces
{
  public interface IPieRepository
  {
    #region Properties

    #endregion

    #region Methods

    Task<IEnumerable<Pie>> AllPies();

    Task<IEnumerable<Pie>> PiesOfTheWeek();

    Task<Pie?> GetPieById(int pieId_);

    Task<IEnumerable<Pie>> AllPiesByCategoryName(string categoryName_);

    Task<IEnumerable<Pie>> SearchPies(string searchQuery_);

    #endregion
  }
}
