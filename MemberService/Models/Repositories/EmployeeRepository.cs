using MemberService.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedModels.Entities;

namespace MemberService.Models.Repositories
{
  public class EmployeeRepository : IEmployeeRepository
  {
    private readonly MemberServiceDbContext _memberServiceDbContext;

    public EmployeeRepository(MemberServiceDbContext memberServiceDbContext_)
    {
      _memberServiceDbContext = memberServiceDbContext_;
    }

    public async Task<IEnumerable<Employee>> Employees() => await _memberServiceDbContext.Employees
    .Include(e => e.Country).Include(e => e.JobCategory).ToListAsync();

    public async Task<Employee?> GetEmployee(int employeeId_) => await _memberServiceDbContext.Employees
    .Where(s => s.EmployeeId == employeeId_).FirstOrDefaultAsync();

		public async Task<bool> AddEmployee(Employee employee_)
		{
      await _memberServiceDbContext.Employees.AddAsync(employee_);

			return await _memberServiceDbContext.SaveChangesAsync() > 0;
		}

    public async Task<bool> UpdateEmployee(Employee employee_)
    {
      await Task.Run(() => _memberServiceDbContext.Employees.Update(employee_));

      return await _memberServiceDbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteEmployee(int employeeId_)
		{
      var employee = await GetEmployee(employeeId_);

      if(employee != null)
      {
        await Task.Run(() => _memberServiceDbContext.Employees.Remove(employee));
      }

      return await _memberServiceDbContext.SaveChangesAsync() > 0;
    }
  }
}
