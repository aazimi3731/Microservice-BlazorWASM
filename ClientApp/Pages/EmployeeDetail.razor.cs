using ClientApp.Interfaces;
using ClientApp.Models.Dtos;
using MapService.Map;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Pages
{
  public partial class EmployeeDetail : ComponentBase
  {
    [Inject]
    private IEmployeeService MemberService { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [Parameter]
		public string? EmployeeId { get; set; }

    public EmployeeDto? Employee { get; set; } = new EmployeeDto();

    public string? Url { get; set; } = string.Empty;

    public List<Marker> MapMarkers { get; set; } = new List<Marker>();

    protected async override Task OnInitializedAsync()
    {
      Employee = await MemberService.GetEmployee(int.Parse(EmployeeId));

      Url = NavigationManager.BaseUri;

      MapMarkers = new List<Marker>
      {
        new Marker
        {
          Description = $"{Employee.FirstName} {Employee.LastName}",
          ShowPopup = false, X = Employee.Longitude, Y = Employee.Latitude
        }
      };

      await base.OnInitializedAsync();
    }
  }
}
