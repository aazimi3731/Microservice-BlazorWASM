using ClientApp.Models.Dtos;
using ClientApp.Interfaces;
using MicroServices.Grpc;
using AutoMapper;

namespace ClientApp.Services
{
  public class EmployeeService : IEmployeeService
  {
    private readonly Members.MembersClient _memberClient;
    private readonly IMapper _mapper;

    public EmployeeService(
      Members.MembersClient memberClient_,
      IMapper mapper_
    )
    {
      _memberClient = memberClient_;
      _mapper = mapper_;
    }

    public async Task<List<EmployeeDto>> GetEmployees()
    {
      var request = new AllEmployeesRequest();

      try
      {
        var response = await _memberClient.AllEmployeesAsync(request);

        if (response == null)
        {
          throw new Exception("Do not receive the response or the response is null for getting the employees list.");
        }

        if (response.IsSuccess && response.Employees.Any())
        {
          return _mapper.Map<List<EmployeeDto>>(response.Employees);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

      return new List<EmployeeDto>();
    }

    public async Task<EmployeeDto?> GetEmployee(int employeeId_)
    {
      var request = new GetEmployeeRequest
      {
        EmployeeId = employeeId_,
      };

      try
      {
        var response = await _memberClient.GetEmployeeAsync(request);

        if (response == null)
        {
          throw new Exception("Do not receive the response or the response is null for getting the employee.");
        }

        if (response.IsSuccess)
        {
          return _mapper.Map<EmployeeDto>(response.Employee);
        }

        return new EmployeeDto();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);

        return new EmployeeDto();
      }
    }

    public async Task<bool> CreateEmployee(EmployeeDto employee_)
    {
      try
      {
        var createEmployeeRequest = new CreateEmployeeRequest
        {
          Employee = _mapper.Map<Employee>(employee_)
        };

        var employeeResponse = await _memberClient.CreateEmployeeAsync(createEmployeeRequest);

        if (employeeResponse == null || !employeeResponse.IsSuccess)
        {
          throw new Exception("Do not receive the response or the response is null for creating employee.");
        }

        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return false;
      }
    }

    public async Task<bool> UpdateEmployee(EmployeeDto employee_)
    {
      try
      {
        var updateEmployeeRequest = new UpdateEmployeeRequest
        {
          Employee = _mapper.Map<Employee>(employee_)
        };

        var employeeResponse = await _memberClient.UpdateEmployeeAsync(updateEmployeeRequest);

        if (employeeResponse == null || !employeeResponse.IsSuccess)
        {
          throw new Exception("Do not receive the response or the response is null for creating employee.");
        }

        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return false;
      }
    }

    public async Task<bool> DeleteEmployee(int employeeId_)
    {
      try
      {
        var deleteEmployeeRequest = new DeleteEmployeeRequest
        {
          EmployeeId = employeeId_
        };
        
        var employeeResponse = await _memberClient.DeleteEmployeeAsync(deleteEmployeeRequest);

        if (employeeResponse == null || !employeeResponse.IsSuccess)
        {
          throw new Exception("Do not receive the response or the response is null for creating employee.");
        }

        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return false;
      }
    }
  }
}
