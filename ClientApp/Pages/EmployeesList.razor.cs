using ClientApp.Components;
using ClientApp.Interfaces;
using ClientApp.Models.Dtos;
using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ClientApp.Pages
{
  public partial class EmployeesList : ComponentBase
  {
    [Inject]
    private IEmployeeService? EmployeeService { get; set; }

    //[Inject]
    //private AuthenticationStateProvider GetAuthenticationStateAsync { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    protected AddEmployeeDialog AddEmployeeDialog { get; set; }

    private List<EmployeeDto>? Employees { get; set; } = new List<EmployeeDto>();

    private bool IsAuthorized { get; set; }

    public string? Url { get; set; } = string.Empty;

    protected async override Task OnInitializedAsync()
    {
      Employees = await EmployeeService?.GetEmployees();

      //      var authstate = await GetAuthenticationStateAsync.GetAuthenticationStateAsync();
      //    var user = authstate != null && authstate.User != null ? authstate.User : null;
      //  var userIdentity = user != null && user.Identity != null ? user.Identity : null;
      //IsAuthorized = userIdentity != null && user.Identity.IsAuthenticated && (user.IsInRole("Admin") || user.IsInRole("Manager"));

      IsAuthorized = true;

      Url = NavigationManager.BaseUri;

      base.OnInitializedAsync();
    }

    protected async Task DeleteEmployee(int employeeId_)
    {
      await EmployeeService?.DeleteEmployee(employeeId_);

      Employees = await EmployeeService?.GetEmployees();
      StateHasChanged();
    }

    protected void QuickAddEmployee()
    {
      AddEmployeeDialog.Show();
    }

    public async void AddEmployeeDialog_OnDialogClose()
    {
      Employees = await EmployeeService?.GetEmployees();
      StateHasChanged();
    }
  }
}
