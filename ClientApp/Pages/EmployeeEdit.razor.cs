using ClientApp.Interfaces;
using ClientApp.Models.Dtos;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Pages
{
  public partial class EmployeeEdit : ComponentBase
  {
    [Inject]
    private ICountryService CountryService { get; set; }

    [Inject]
    private IJobCategoryService JobCategoryService { get; set; }

    [Inject]
    private IEmployeeService EmployeeService { get; set; }

    [Parameter]
    public string EmployeeId { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    private EmployeeDto Employee { get; set; } = new EmployeeDto();
    private List<CountryDto> Countries { get; set; } = new List<CountryDto>();
    private List<JobCategoryDto> JobCategories { get; set; } = new List<JobCategoryDto>();

    protected string CountryId = string.Empty;
    protected string JobCategoryId = string.Empty;

    //used to store state of screen
    protected string Message = string.Empty;
    protected string StatusClass = string.Empty;
    protected bool Saved;

    protected override async Task OnInitializedAsync()
    {
      Saved = false;
      int.TryParse(EmployeeId, out var employeeId);

      Countries = await CountryService.GetCountries();
      JobCategories = await JobCategoryService.GetJobCategories();

      if (employeeId == 0)
      {
        Employee = new EmployeeDto { CountryId = 1, JobCategoryId = 1, BirthDate = DateTime.Now, JoinedDate = DateTime.Now };

        CountryId = "1";
        JobCategoryId = "1";
      }
      else
      {
        Employee = await EmployeeService.GetEmployee(employeeId);

        CountryId = Employee.CountryId.ToString();
        JobCategoryId = Employee.JobCategoryId.ToString();
      }
    }

    protected async Task HandleValidSubmit()
    {
      Saved = false;
      Employee.CountryId = int.Parse(CountryId);
      Employee.JobCategoryId = int.Parse(JobCategoryId);

      if (Employee.EmployeeId == 0)
      {
        var isEmployeeAdded = await EmployeeService.CreateEmployee(Employee);
        if (isEmployeeAdded)
        {
          StatusClass = "alert-success";
          Message = "New employee added successfully.";
          Saved = true;
        }
        else
        {
          StatusClass = "alert-danger";
          Message = "Something went wrong adding the new employee. Please try again.";
          Saved = false;
        }
      }
      else
      {
        await EmployeeService.UpdateEmployee(Employee);
        StatusClass = "alert-success";
        Message = "Employee updated successfully.";
        Saved = true;
      }
    }

    protected void HandleInvalidSubmit()
    {
      StatusClass = "alert-danger";
      Message = "There are some validation errors. Please try again.";
    }

    protected async Task DeleteEmployee()
    {
      await EmployeeService.DeleteEmployee(Employee.EmployeeId);

      StatusClass = "alert-success";
      Message = "Deleted successfully";

      Saved = true;
    }

    protected void NavigateToEmployeesList()
    {
      NavigationManager.NavigateTo("/employeeslist");
    }
  }
}
