using Microsoft.EntityFrameworkCore;
using SharedModels.Entities;
using ProductCategoryService.Models.Interfaces;

namespace ProductCategoryService.Models.Repositories
{
  public class CategoryRepository : ICategoryRepository
  {
    private readonly ProductCategoryServiceDbContext _productCategoryServiceDbContext;

    public CategoryRepository(ProductCategoryServiceDbContext productCategoryServiceDbContext_)
    {
      _productCategoryServiceDbContext = productCategoryServiceDbContext_;
    }

    public async Task<IEnumerable<Category>> AllCategories() => await _productCategoryServiceDbContext.Categories.ToListAsync();
  }
}
