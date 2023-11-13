using ClientApp.Models.Dtos;
namespace ClientApp.Interfaces
{
  public interface IEmployeeService
  {
    Task<EmployeeDto?> GetEmployee(int employeeId_);

    Task<List<EmployeeDto>> GetEmployees();

    Task<bool> CreateEmployee(EmployeeDto employee_);

    Task<bool> UpdateEmployee(EmployeeDto employee_);

    Task<bool> DeleteEmployee(int employeeId_);
  }
}
