using AutoMapper;
using MemberService.Interfaces;
using MemberService.Models.Interfaces;
using MicroServices.Grpc;

namespace MemberService.Services
{
  public class JobCategoryService : IJobCategoryService
  {
    private readonly IJobCategoryRepository _jobCategoryRepository;
    private readonly IMapper _mapper;

    public JobCategoryService(
      IJobCategoryRepository jobCategoryRepository_,
      IMapper mapper_
    ) {
      _jobCategoryRepository = jobCategoryRepository_;
      _mapper = mapper_;
    }

    public async Task<IEnumerable<JobCategory>> JobCategories()
    {
      try
      {
        var jobCategories = await _jobCategoryRepository.JobCategories();

        return _mapper.Map<IEnumerable<JobCategory>>(jobCategories).ToList();
      }
      catch (Exception ex)
      {
        return Enumerable.Empty<JobCategory>();
      }
    }

    public async Task<JobCategory?> GetJobCategory(int jobCategoryId_)
    {
      try
      {
        var jobCategory = await _jobCategoryRepository.GetJobCategory(jobCategoryId_);

        return _mapper.Map<JobCategory>(jobCategory);
      }
      catch (Exception ex)
      {
        return new JobCategory();
      }
    }
  }
}
