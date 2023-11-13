using ClientApp.Models.Dtos;
using ClientApp.Interfaces;
using MicroServices.Grpc;
using AutoMapper;

namespace ClientApp.Services
{
  public class JobCategoryService : IJobCategoryService
  {
    private readonly Members.MembersClient _memberClient;
    private readonly IMapper _mapper;

    public JobCategoryService(
      Members.MembersClient memberClient_,
      IMapper mapper_
    )
    {
      _memberClient = memberClient_;
      _mapper = mapper_;
    }

    public async Task<List<JobCategoryDto>> GetJobCategories()
    {
      var request = new AllJobCategoriesRequest();

      try
      {
        var response = await _memberClient.AllJobCategoriesAsync(request);

        if (response == null)
        {
          throw new Exception("Do not receive the response or the response is null for getting the job categories list.");
        }

        if (response.IsSuccess && response.JobCategories.Any())
        {
          return _mapper.Map<List<JobCategoryDto>>(response.JobCategories);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

      return new List<JobCategoryDto>();
    }

    public async Task<JobCategoryDto?> GetJobCategory(int jobCategoryId_)
    {
      var request = new GetJobCategoryRequest
      {
        JobCategoryId = jobCategoryId_
      };

      try
      {
        var response = await _memberClient.GetJobCategoryAsync(request);

        if (response == null)
        {
          throw new Exception("Do not receive the response or the response is null for getting the job category.");
        }

        if (response.IsSuccess)
        {
          return _mapper.Map<JobCategoryDto>(response.JobCategory);
        }

        return new JobCategoryDto();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);

        return new JobCategoryDto();
      }
    }
  }
}
