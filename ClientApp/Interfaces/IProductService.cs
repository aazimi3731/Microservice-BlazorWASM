using ClientApp.Models.Dtos;

namespace ClientApp.Interfaces
{
    public interface IProductService
  {
    Task<IEnumerable<PieDto>> AllPies();

    Task<IEnumerable<PieDto>> PiesOfTheWeek();

    Task<IEnumerable<PieDto>> AllPiesByCategoryName(string categoryName_);

    Task<PieDto> GetPieById(int pieId_);

    Task<IEnumerable<PieDto>> SearchPies(string searchQuery_);
  }
}
