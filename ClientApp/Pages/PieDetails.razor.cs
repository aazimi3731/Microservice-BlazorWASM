using ClientApp.Interfaces;
using ClientApp.Models.Dtos;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Pages
{
  public partial class PieDetails : ComponentBase
  {
    [Inject]
    private IProductService ProductService { get; set; }

    [Parameter]
    public string PieId { get; set; }

    public PieDto? Pie { get; set; }


    protected override async Task OnInitializedAsync()
    {
      Pie = await ProductService.GetPieById(int.Parse(PieId));

      await base.OnInitializedAsync();
    }
  }
}
