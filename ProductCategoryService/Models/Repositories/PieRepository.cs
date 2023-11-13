using Microsoft.EntityFrameworkCore;
using SharedModels.Entities;
using ProductCategoryService.Models.Interfaces;

namespace ProductCategoryService.Models.Repositories
{
  public class PieRepository : IPieRepository
  {
    private readonly ProductCategoryServiceDbContext _productCategoryServiceDbContext;

    public PieRepository(ProductCategoryServiceDbContext productCategoryServiceDbContext_)
    {
      _productCategoryServiceDbContext = productCategoryServiceDbContext_;
    }

    public async Task<IEnumerable<Pie>> AllPies() => await _productCategoryServiceDbContext.Pies.Include(p => p.Category).ToListAsync();

    public async Task<IEnumerable<Pie>> PiesOfTheWeek() => await _productCategoryServiceDbContext.Pies.Include(p => p.Category)
      .Where(p => p.IsPieOfTheWeek).ToListAsync();

    public async Task<Pie?> GetPieById(int pieId_) => await _productCategoryServiceDbContext.Pies.Include(p => p.Category)
      .FirstOrDefaultAsync(p => p.PieId == pieId_);

    public async Task<IEnumerable<Pie>> AllPiesByCategoryName(string categoryName_) => await _productCategoryServiceDbContext.Pies.Include(p => p.Category)
      .Where(p => p.Category.CategoryName.Equals(categoryName_)).ToListAsync();

    public async Task<IEnumerable<Pie>> SearchPies(string searchQuery_) => await _productCategoryServiceDbContext.Pies.Include(p => p.Category)
      .Where(p => p.Name.ToLower().Contains(searchQuery_.ToLower())).ToListAsync();
  }
}
