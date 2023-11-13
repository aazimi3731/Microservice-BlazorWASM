using MemberService.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedModels.Entities;

namespace MemberService.Models.Repositories
{
  public class JobCategoryRepository : IJobCategoryRepository
  {
    private readonly MemberServiceDbContext _memberServiceDbContext;

    public JobCategoryRepository(MemberServiceDbContext memberServiceDbContext_)
    {
      _memberServiceDbContext = memberServiceDbContext_;
    }

    public async Task<IEnumerable<JobCategory>> JobCategories() => await _memberServiceDbContext.JobCategories.ToListAsync();

    public async Task<JobCategory?> GetJobCategory(int jobCategoryId_) => await _memberServiceDbContext.JobCategories
    .Where(s => s.JobCategoryId == jobCategoryId_).FirstOrDefaultAsync();
  }
}
