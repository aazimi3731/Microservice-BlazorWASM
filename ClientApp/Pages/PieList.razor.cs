using ClientApp.Interfaces;
using ClientApp.Models.Dtos;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Pages
{
  public partial class PieList
  {
    [Inject]
    private IProductService ProductService { get; set; }

    [Inject]
    private ICategoryService CategoryService { get; set; }

    [Parameter]
    public string? CategoryName { get; set; }

    public string? CurrentCategory { get; set; }

    public IEnumerable<PieDto>? Pies { get; set; }

    protected override async Task OnInitializedAsync()
    {
      await CreatePiesList();

      await base.OnInitializedAsync();
    }

    protected override async void OnParametersSet()
    {
      await CreatePiesList();

      StateHasChanged();

      base.OnParametersSet();
    }

    private async Task CreatePiesList()
    {
      IEnumerable<PieDto> pies;

      if (string.IsNullOrEmpty(CategoryName))
      {
        pies = await ProductService.AllPies();

        CurrentCategory = "All pies";
      }
      else
      {
        pies = await ProductService.AllPiesByCategoryName(CategoryName);

        var categories = await CategoryService.AllCategories();

        CurrentCategory = categories.FirstOrDefault(c => c.CategoryName == CategoryName)?.CategoryName;
      }

      Pies = pies.OrderBy(p => p.PieId);
    }
  }
}
