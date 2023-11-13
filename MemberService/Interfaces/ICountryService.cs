using MicroServices.Grpc;

namespace MemberService.Interfaces
{
  public interface ICountryService
  {
    Task<IEnumerable<Country>> Countries();

    Task<Country?> GetCountry(int countryId_);
	}
}
