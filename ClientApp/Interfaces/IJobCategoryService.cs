using ClientApp.Models.Dtos;
namespace ClientApp.Interfaces
{
  public interface IJobCategoryService
  {
    Task<JobCategoryDto?> GetJobCategory(int jobCategoryId_);

    Task<List<JobCategoryDto>> GetJobCategories();
  }
}
