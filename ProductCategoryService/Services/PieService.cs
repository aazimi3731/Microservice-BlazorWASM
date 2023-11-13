using SharedModels.Entities;
using ProductCategoryService.Models.Interfaces;

namespace ProductCategoryService.Models.Repositories
{
  public class PieService : IPieService
  {
    private readonly IPieRepository _pieRepository;

    public PieService(IPieRepository pieRepository_)
    {
      _pieRepository = pieRepository_;
    }

    public async Task<IEnumerable<Pie>> AllPies() => await _pieRepository.AllPies();

    public async Task<IEnumerable<Pie>> PiesOfTheWeek() => await _pieRepository.PiesOfTheWeek();

    public async Task<Pie?> GetPieById(int pieId_) => await _pieRepository.GetPieById(pieId_);

    public async Task<IEnumerable<Pie>> AllPiesByCategoryName(string categoryName_) => await _pieRepository.AllPiesByCategoryName(categoryName_);

    public async Task<IEnumerable<Pie>> SearchPies(string searchQuery_) => await _pieRepository.SearchPies(searchQuery_);
  }
}
