using ClientApp.Models.Dtos;
using ClientApp.Interfaces;
using MicroServices.Grpc;
using AutoMapper;

namespace ClientApp.Services
{
  public class CountryService : ICountryService
  {
    private readonly Members.MembersClient _memberClient;
    private readonly IMapper _mapper;

    public CountryService(
      Members.MembersClient memberClient_,
      IMapper mapper_
    ) {
      _memberClient = memberClient_;
      _mapper = mapper_;
    }

    public async Task<List<CountryDto>> GetCountries()
    {
      var request = new AllCountriesRequest();

      try
      {
        var response = await _memberClient.AllCountriesAsync(request);

        if (response == null)
        {
          throw new Exception("Do not receive the response or the response is null for getting the countries list.");
        }

        if (response.IsSuccess && response.Countries.Any())
        {
          return _mapper.Map<List<CountryDto>>(response.Countries);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

      return new List<CountryDto>();
    }

    public async Task<CountryDto?> GetCountry(int countryId_)
    {
      var request = new GetCountryRequest
      {
        CountryId = countryId_
      };

      try
      {
        var response = await _memberClient.GetCountryAsync(request);

        if (response == null)
        {
          throw new Exception("Do not receive the response or the response is null for getting the country.");
        }

        if (response.IsSuccess)
        {
          return _mapper.Map<CountryDto>(response.Country);
        }

        return new CountryDto();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);

        return new CountryDto();
      }
    }
  }
}
