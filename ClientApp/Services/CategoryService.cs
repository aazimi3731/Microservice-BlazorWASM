using AutoMapper;
using ClientApp.Interfaces;
using ClientApp.Models.Dtos;
using SharedModels.Entities;

namespace ClientApp.Services
{
    public class CategoryService : ICategoryService
  {
    private readonly IMicroserviceReqResp _microserviceReqResp;
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public CategoryService(
      IMicroserviceReqResp microserviceReqResp_,
      IProductService productService_, 
      IMapper mapper_
    ) {
      _microserviceReqResp = microserviceReqResp_;
      _productService = productService_;
      _mapper = mapper_;
    }

    public async Task<IEnumerable<CategoryDto>> AllCategories()
    {
      try
      {
        var categories = await _microserviceReqResp.SendGetRequest<List<Category>>("api/category/Categories");

        if (categories == null || !categories.Any())
        {
          throw new ApplicationException($"The category list is null or empty.");
        }
        
        var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

        foreach (var category in categoryDtos)
        {
          var pies = await _productService.AllPiesByCategoryName(category.CategoryName);

          category.Pies = pies.OrderBy(p => p.PieId).OrderBy(p => p.PieId).ToList();
        }

        return _mapper.Map<IEnumerable<CategoryDto>>(categoryDtos) ?? new List<CategoryDto>();
      }
      catch (ApplicationException ex)
      {
        Console.WriteLine("An error occured during sending request to get categories", ex.ToString());
        return new List<CategoryDto>();
      }
    }
  }
}
