using SharedModels.Entities;

namespace MemberService.Models.Interfaces
{
  public interface ICountryRepository
  {
    Task<IEnumerable<Country>> Countries();

    Task<Country?> GetCountry(int countryId_);
	}
}
