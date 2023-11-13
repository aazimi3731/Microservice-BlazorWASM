using SharedModels.Entities;

namespace MemberService.Models.Interfaces
{
  public interface IEmployeeRepository
  {
    Task<IEnumerable<Employee>> Employees();

    Task<Employee?> GetEmployee(int employeeId_);

		Task<bool> AddEmployee(Employee employee_);
		
		Task<bool> UpdateEmployee(Employee employee_);
		
		Task<bool> DeleteEmployee(int employeeId_);
	}
}
