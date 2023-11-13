using ClientApp.Interfaces;
using ClientApp.Models.Dtos;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components
{
  public partial class CategoryMenu : ComponentBase
  {
    [Inject]
    public ICategoryService? CategoryService { get; set; }

    public IEnumerable<CategoryDto>? CategoriesList { get; set; }

    protected override async Task OnInitializedAsync()
    {
      var categories = await CategoryService?.AllCategories();
      
      CategoriesList = categories.OrderBy(c => c.CategoryName);
    }
  }
}
