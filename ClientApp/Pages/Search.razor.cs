using Microsoft.AspNetCore.Components;
using ClientApp.Interfaces;
using ClientApp.Models.Dtos;

namespace ClientApp.Pages
{
  public partial class Search
  {
    public string SearchText { get; set; } = string.Empty;

    public List<PieDto> FilteredPies { get; set; } = new List<PieDto>();

    [Inject]
    private IProductService? ProductService { get; set; }

    private async Task SearchPie()
    {
      FilteredPies.Clear();
      if (ProductService is not null)
      {
        if (SearchText.Length >= 3)
        {
          var response = await ProductService.SearchPies(SearchText);

          FilteredPies = response.ToList();
        }
      }
    }
  }
}
