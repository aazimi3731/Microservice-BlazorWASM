using Microsoft.AspNetCore.Mvc;
using SharedModels.Entities;
using ProductCategoryService.Models.Interfaces;

namespace ProductCategoryService.Controllers
{
  [Route("api/[controller]/[Action]")]
  [ApiController]

  public class PieController : ControllerBase
  {
    private readonly IPieService _pieService;

    public PieController(IPieService pieService_)
    {
      _pieService = pieService_;
    }

    //public ActionResult Pies()
    //{
    //  //ViewBag.CurrentCategory = "Cheese cakes";

    //  ////Pass data using Model
    //  //return View(_pieRepository.AllPies);

    //  PieListViewModel pieListViewModel = new PieListViewModel(_pieRepository.AllPies, "Cheese cakes");

    //  //Pass data using Model by getting data from ViewModel
    //  return View(pieListViewModel);
    //}

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pie>>> Pies([FromQuery] string? categoryName) 
    {
      IEnumerable<Pie> pies;

      if (string.IsNullOrEmpty(categoryName))
      {
        pies = await _pieService.AllPies();
      }
      else
      {
        pies = await _pieService.AllPiesByCategoryName(categoryName);
      }

      return Ok(pies);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pie>>> PiesOfTheWeek()
    {
      var pies = await _pieService.PiesOfTheWeek();

      return Ok(pies);
    }

    //[HttpGet("{pieId:int}")] --> it does not work and it conflict with string in the request

    [HttpGet]
    public async Task<ActionResult<Pie>> PieDetails([FromQuery] int pieId)
    {
      if (pieId <= 0)
      {
        return NotFound();
      }

      var pie = await _pieService.GetPieById(pieId);

      if (pie == null)
      {
        return NotFound();
      }

      return Ok(pie);
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pie>>> SearchPies([FromQuery] string? searchQuery)
    {
      var pies = await _pieService.SearchPies(searchQuery ?? string.Empty);

      return Ok(pies);
    }
  }
}
