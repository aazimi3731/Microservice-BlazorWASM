using AutoMapper;
using MemberService.Interfaces;
using MemberService.Models.Interfaces;
using MicroServices.Grpc;

namespace MemberService.Services
{
  public class CountryService : ICountryService
  {
    private readonly ICountryRepository _countryRepository;
    private readonly IMapper _mapper;

    public CountryService(
      ICountryRepository countryRepository_,
      IMapper mapper_
    ) {
      _countryRepository = countryRepository_;
      _mapper = mapper_;
    }

    public async Task<IEnumerable<Country>> Countries()
    {
      try
      {
        var countries = await _countryRepository.Countries();

        return _mapper.Map<IEnumerable<Country>>(countries).ToList();
      }
      catch (Exception ex)
      {
        return Enumerable.Empty<Country>();
      }
    }

    public async Task<Country?> GetCountry(int countryId_)
    {
      try
      {
        var country = await _countryRepository.GetCountry(countryId_);

        return _mapper.Map<Country>(country);
      }
      catch (Exception ex)
      {
        return new Country();
      }
    }
  }
}
