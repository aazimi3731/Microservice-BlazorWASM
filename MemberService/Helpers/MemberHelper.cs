using MicroServices.Grpc;
using Grpc.Core;
using MemberService.Interfaces;

namespace MemberService.Services
{
  public class MemberHelper : Members.MembersBase
  {
    private readonly ICountryService _countryService;
    private readonly IJobCategoryService _jobCategoryService;
    private readonly IEmployeeService _employeeService;

    public MemberHelper(
      ICountryService countryService_,
      IJobCategoryService jobCategoryService_,
      IEmployeeService employeeService_
    ) {
      _countryService = countryService_;
      _jobCategoryService = jobCategoryService_;
      _employeeService = employeeService_;
    }

    public override async Task<AllEmployeesResponse> AllEmployees(AllEmployeesRequest request_, ServerCallContext context)
    {
      var response = new AllEmployeesResponse();

      if (request_ == null)
      {
        response.IsSuccess = false;

        return response;
      }
      try
      {
        var employees = await _employeeService.Employees();

        response.IsSuccess = false;
        if (employees.Any())
        {
          response.Employees.AddRange(employees);
          response.IsSuccess = true;
        }
      }
      catch (Exception ex)
      {
        response.IsSuccess = false;
      }

      return response;
    }

    public override async Task<GetEmployeeResponse> GetEmployee(GetEmployeeRequest request_, ServerCallContext context)
    {
      var response = new GetEmployeeResponse();

      if (request_ == null)
      {
        response.IsSuccess = false;

        return response;
      }
      try
      {
        var employee = await _employeeService.GetEmployee(request_.EmployeeId);

        response.IsSuccess = false;
        if (employee != null && employee.GetType().GetProperties().Any())
        {
          response.Employee = employee;
          response.IsSuccess = true;
        }
      }
      catch (Exception ex)
      {
        response.IsSuccess = false;
      }

      return response;
    }

    public override async Task<CreateEmployeeResponse> CreateEmployee(CreateEmployeeRequest request_, ServerCallContext context)
    {
      var response = new CreateEmployeeResponse();

      if (request_ == null)
      {
        response.IsSuccess = false;

        return response;
      }
      try
      {
        response.IsSuccess = await _employeeService.AddEmployee(request_.Employee);
      }
      catch (Exception ex)
      {
        response.IsSuccess = false;
      }

      return response;
    }

    public override async Task<UpdateEmployeeResponse> UpdateEmployee(UpdateEmployeeRequest request_, ServerCallContext context)
    {
      var response = new UpdateEmployeeResponse();

      if (request_ == null)
      {
        response.IsSuccess = false;

        return response;
      }
      try
      {
        response.IsSuccess = await _employeeService.UpdateEmployee(request_.Employee);
      }
      catch (Exception ex)
      {
        response.IsSuccess = false;
      }

      return response;
    }

    public override async Task<DeleteEmployeeResponse> DeleteEmployee(DeleteEmployeeRequest request_, ServerCallContext context)
    {
      var response = new DeleteEmployeeResponse();

      if (request_ == null)
      {
        response.IsSuccess = false;

        return response;
      }
      try
      {
        response.IsSuccess = await _employeeService.DeleteEmployee(request_.EmployeeId);
      }
      catch (Exception ex)
      {
        response.IsSuccess = false;
      }

      return response;
    }

    public override async Task<AllCountriesResponse> AllCountries(AllCountriesRequest request_, ServerCallContext context)
    {
      var response = new AllCountriesResponse();

      if (request_ == null)
      {
        response.IsSuccess = false;

        return response;
      }
      try
      {
        var countries = await _countryService.Countries();

        response.IsSuccess = false;
        if (countries.Any())
        {
          response.Countries.AddRange(countries);
          response.IsSuccess = true;
        }
      }
      catch (Exception ex)
      {
        response.IsSuccess = false;
      }

      return response;
    }

    public override async Task<GetCountryResponse> GetCountry(GetCountryRequest request_, ServerCallContext context)
    {
      var response = new GetCountryResponse();

      if (request_ == null)
      {
        response.IsSuccess = false;

        return response;
      }
      try
      {
        var country = await _countryService.GetCountry(request_.CountryId);

        response.IsSuccess = false;
        if (country != null && country.GetType().GetProperties().Any())
        {
          response.Country = country;
          response.IsSuccess = true;
        }
      }
      catch (Exception ex)
      {
        response.IsSuccess = false;
      }

      return response;
    }

    public override async Task<AllJobCategoriesResponse> AllJobCategories(AllJobCategoriesRequest request_, ServerCallContext context)
    {
      var response = new AllJobCategoriesResponse();

      if (request_ == null)
      {
        response.IsSuccess = false;

        return response;
      }
      try
      {
        var jobCategories = await _jobCategoryService.JobCategories();

        response.IsSuccess = false;
        if (jobCategories.Any())
        {
          response.JobCategories.AddRange(jobCategories);
          response.IsSuccess = true;
        }
      }
      catch (Exception ex)
      {
        response.IsSuccess = false;
      }

      return response;
    }

    public override async Task<GetJobCategoryResponse> GetJobCategory(GetJobCategoryRequest request_, ServerCallContext context)
    {
      var response = new GetJobCategoryResponse();

      if (request_ == null)
      {
        response.IsSuccess = false;

        return response;
      }
      try
      {
        var jobCategory = await _jobCategoryService.GetJobCategory(request_.JobCategoryId);

        response.IsSuccess = false;
        if (jobCategory != null && jobCategory.GetType().GetProperties().Any())
        {
          response.JobCategory = jobCategory;
          response.IsSuccess = true;
        }
      }
      catch (Exception ex)
      {
        response.IsSuccess = false;
      }

      return response;
    }
  }
}
