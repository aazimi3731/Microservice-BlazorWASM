using AutoMapper;
using ClientApp.Interfaces;
using ClientApp.Models.Dtos;
using SharedModels.Entities;

namespace ClientApp.Services
{
    public class ProductService : IProductService
  {
    private readonly IMicroserviceReqResp _microserviceReqResp;
    private readonly IMapper _mapper;

    public ProductService(IMicroserviceReqResp microserviceReqResp_, IMapper mapper_)
    {
      _microserviceReqResp = microserviceReqResp_;
      _mapper = mapper_;
    }

    public async Task<IEnumerable<PieDto>> AllPies()
    {
      try
      {
        var pies = await _microserviceReqResp.SendGetRequest<List<Pie>>("api/pie/pies/?categoryName=");

        if (pies == null || !pies.Any())
        {
          throw new ApplicationException($"The pie list is null or empty.");
        }

        return _mapper.Map<IEnumerable<PieDto>>(pies);
      }
      catch (ApplicationException ex)
      {
        Console.WriteLine("An error occured during sending request to get pies", ex.ToString());
        return new List<PieDto>();
      }
    }

    public async Task<IEnumerable<PieDto>> AllPiesByCategoryName(string categoryName_)
    {
      try
      {
        var pies = await _microserviceReqResp.SendGetRequest<List<Pie>>($"api/pie/pies/?categoryName={categoryName_}");

        if (pies == null || !pies.Any())
        {
          throw new ApplicationException($"The pie list is null or empty.");
        }

        return _mapper.Map<IEnumerable<PieDto>>(pies) ?? new List<PieDto>();
      }
      catch (ApplicationException ex)
      {
        Console.WriteLine("An error occured during sending request to get pies", ex.ToString());
        return new List<PieDto>();
      }
    }

    public async Task<PieDto> GetPieById(int pieId_)
    {
      try
      {
        var pie = await _microserviceReqResp.SendGetRequest<Pie>($"api/pie/PieDetails/?pieId={pieId_}");

        if (pie == null)
        {
          throw new ApplicationException($"The pie is null.");
        }

        return _mapper.Map<PieDto>(pie) ?? new PieDto();
      }
      catch (ApplicationException ex)
      {
        Console.WriteLine("An error occured during sending request to get pie", ex.ToString());
        return new PieDto();
      }
    }

    public async Task<IEnumerable<PieDto>> PiesOfTheWeek()
    {
      try
      {
        var pies = await _microserviceReqResp.SendGetRequest<List<Pie>>("api/pie/piesoftheweek");

        if (pies == null || !pies.Any())
        {
          throw new ApplicationException($"The pie list is null or empty.");
        }

        return _mapper.Map<IEnumerable<PieDto>>(pies) ?? new List<PieDto>();
      }
      catch (ApplicationException ex)
      {
        Console.WriteLine("An error occured during sending request to get pies", ex.ToString());
        return new List<PieDto>();
      }
    }

    public async Task<IEnumerable<PieDto>> SearchPies(string searchQuery_)
    {
      try
      {
        var pies = await _microserviceReqResp.SendGetRequest<List<Pie>>($"api/pie/searchpies/?searchQuery={searchQuery_}");

        if (pies == null || !pies.Any())
        {
          throw new ApplicationException($"The pie list is null or empty.");
        }

        return _mapper.Map<IEnumerable<PieDto>>(pies) ?? new List<PieDto>();
      }
      catch (ApplicationException ex)
      {
        Console.WriteLine("An error occured during sending request to get pies", ex.ToString());
        return new List<PieDto>();
      }
    }
  }
}
