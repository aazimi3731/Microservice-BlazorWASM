using ClientApp.Models.Dtos;
namespace ClientApp.Interfaces
{
  public interface ICountryService
  {
    Task<CountryDto?> GetCountry(int countryId_);

    Task<List<CountryDto>> GetCountries();
  }
}
