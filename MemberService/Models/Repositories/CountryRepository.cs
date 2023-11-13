using MemberService.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedModels.Entities;

namespace MemberService.Models.Repositories
{
  public class CountryRepository : ICountryRepository
  {
    private readonly MemberServiceDbContext _memberServiceDbContext;

    public CountryRepository(MemberServiceDbContext memberServiceDbContext_)
    {
      _memberServiceDbContext = memberServiceDbContext_;
    }

    public async Task<IEnumerable<Country>> Countries() => await _memberServiceDbContext.Countries.ToListAsync();

    public async Task<Country?> GetCountry(int countryId_) => await _memberServiceDbContext.Countries
    .Where(s => s.CountryId == countryId_).FirstOrDefaultAsync();
	}
}
