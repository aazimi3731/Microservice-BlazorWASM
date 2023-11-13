using MicroServices.Grpc;

namespace MemberService.Interfaces
{
  public interface IJobCategoryService
  {
    Task<IEnumerable<JobCategory>> JobCategories();

    Task<JobCategory?> GetJobCategory(int jobCategoryId_);
	}
}
