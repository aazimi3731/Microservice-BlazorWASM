using SharedModels.Entities;

namespace MemberService.Models.Interfaces
{
  public interface IJobCategoryRepository
  {
    Task<IEnumerable<JobCategory>> JobCategories();

    Task<JobCategory?> GetJobCategory(int jobCategoryId_);
	}
}
