using Microsoft.EntityFrameworkCore;
using SharedModels.Entities;
using ProductCategoryService.Models.Interfaces;

namespace ProductCategoryService.Models.Repositories
{
  public class CategoryService : ICategoryService
  {
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository_)
    {
      _categoryRepository = categoryRepository_;
    }

    public async Task<IEnumerable<Category>> AllCategories() => await _categoryRepository.AllCategories();
  }
}
