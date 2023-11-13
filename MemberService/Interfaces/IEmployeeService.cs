using MicroServices.Grpc;

namespace MemberService.Interfaces
{
  public interface IEmployeeService
  {
    Task<IEnumerable<Employee>> Employees();

    Task<Employee?> GetEmployee(int employeeId_);

		Task<bool> AddEmployee(Employee employee_);
		
		Task<bool> UpdateEmployee(Employee employee_);
		
		Task<bool> DeleteEmployee(int employeeId_);
	}
}
