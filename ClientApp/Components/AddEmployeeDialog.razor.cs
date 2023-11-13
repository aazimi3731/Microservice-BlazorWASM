using ClientApp.Interfaces;
using ClientApp.Models.Dtos;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components
{
  public partial class AddEmployeeDialog
  {
    public EmployeeDto Employee { get; set; } =
        new EmployeeDto { CountryId = 1, JobCategoryId = 1, BirthDate = DateTime.Now, JoinedDate = DateTime.Now };

    [Inject]
    public IEmployeeService? MemberService { get; set; }

    public bool ShowDialog { get; set; }

    [Parameter]
    public EventCallback<bool> CloseEventCallback { get; set; }

    public void Show()
    {
      ResetDialog();
      ShowDialog = true;
      StateHasChanged();
    }

    public void Close()
    {
      ShowDialog = false;
      StateHasChanged();
    }

    private void ResetDialog()
    {
      Employee = new EmployeeDto { CountryId = 1, JobCategoryId = 1, BirthDate = DateTime.Now, JoinedDate = DateTime.Now };
    }

    protected async Task HandleValidSubmit()
    {
      await MemberService.CreateEmployee(Employee);
      ShowDialog = false;

      await CloseEventCallback.InvokeAsync(true);
      StateHasChanged();
    }
  }
}
