using AutoMapper;
using MemberService.Models.Interfaces;
using MemberService.Interfaces;
using EmployeeModel = SharedModels.Entities.Employee;
using MicroServices.Grpc;

namespace MemberService.Services
{
  public class EmployeeService : IEmployeeService
  {
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public EmployeeService(
      IEmployeeRepository employeeRepository_,
      IMapper mapper_
    ) {
      _employeeRepository = employeeRepository_;
      _mapper = mapper_;
    }

    public async Task<IEnumerable<Employee>> Employees()
    {
      try
      {
        var employees = await _employeeRepository.Employees();

        return _mapper.Map<IEnumerable<Employee>>(employees).ToList();
      }
      catch (Exception ex)
      {
        return Enumerable.Empty<Employee>();
      }
    }

    public async Task<Employee?> GetEmployee(int employeeId_)
    {
      try
      {
        var employee = await _employeeRepository.GetEmployee(employeeId_);

        return _mapper.Map<Employee>(employee);
      }
      catch (Exception ex)
      {
        return new Employee();
      }
    }

    public async Task<bool> AddEmployee(Employee employee_)
    {
      if (employee_ == null)
      {
        return false;
      }
      try
      {
        var employee = _mapper.Map<EmployeeModel>(employee_);

        return await _employeeRepository.AddEmployee(employee);
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public async Task<bool> UpdateEmployee(Employee employee_)
    {
      if (employee_ == null)
      {
        return false;
      }
      try
      {
        var employee = _mapper.Map<EmployeeModel>(employee_);

        return await _employeeRepository.UpdateEmployee(employee);
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public async Task<bool> DeleteEmployee(int employeeId_)
    {
      try
      {
        return await _employeeRepository.DeleteEmployee(employeeId_);
      }
      catch (Exception ex)
      {
        return false;
      }
    }
  }
}
