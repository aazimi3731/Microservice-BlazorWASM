using Microsoft.AspNetCore.Mvc;
using SharedModels.Entities;
using ProductCategoryService.Models.Interfaces;

namespace ProductCategoryService.Controllers
{
  [Route("api/[Controller]/[Action]")]
  [ApiController]
  public class CategoryController : ControllerBase
  {
    private ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService_)
    {
      _categoryService = categoryService_;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> Categories()
    {
      var result = await _categoryService.AllCategories();
      return Ok(result);
    }
  }
}
