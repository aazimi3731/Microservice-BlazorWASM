using SharedModels.Entities;

namespace MemberService.Models
{
  public class DbInitializer
  {
    public static void Seed(IApplicationBuilder applicationBuilder)
    {
      MemberServiceDbContext context = applicationBuilder.ApplicationServices.CreateAsyncScope().ServiceProvider.GetRequiredService<MemberServiceDbContext>();

      Console.WriteLine("Test");
      Console.WriteLine(context);

      if (!context.JobCategories.Any())
      {
        context.JobCategories.AddRange(JobCategories.Select(c => c.Value));

        context.SaveChanges();
      }

      if (!context.Countries.Any())
      {
        context.Countries.AddRange(Countries.Select(c => c.Value));

        context.SaveChanges();
      }

      if (!context.Employees.Any())
      {
        context.AddRange
        (
          new Employee
          { 
            CountryId = 1,
            MaritalStatus = MaritalStatus.Single,
            BirthDate = new DateTime(1989, 3, 11),
            City = "Montreal",
            Email = "aazimi3731@gmail.com",
            FirstName = "Aziz",
            LastName = "Azimi",
            Gender = Gender.Male,
            PhoneNumber = "324777888773",
            Smoker = false,
            Street = "Grote Markt 1",
            Zip = "1000",
            JobCategoryId = 1,
            Comment = "Lorem Ipsum",
            ExitDate = null,
            JoinedDate = new DateTime(2015, 3, 1),
						Latitude = 50.8503,
						Longitude = 4.3517
					},
          new Employee
          { 
            CountryId = 2, 
            MaritalStatus = MaritalStatus.Married, 
            BirthDate = new DateTime(1979, 1, 16), 
            City = "Toronto", 
            Email = "nazimi3731@gmail.com", 
            FirstName = "Nazanin", 
            LastName = "Azimi", 
            Gender = Gender.Female, 
            PhoneNumber = "33999909923", 
            Smoker = false, 
            Street = "New Street", 
            Zip = "2000", 
            JobCategoryId = 1, 
            Comment = "Lorem Ipsum", 
            ExitDate = null, 
            JoinedDate = new DateTime(2017, 12, 24),
						Latitude = 51.8503,
						Longitude = 4.5517
					}
				);

        context.SaveChanges();
      }
    }

    private static Dictionary<string, JobCategory>? jobCategories;

    public static Dictionary<string, JobCategory> JobCategories
    {
      get
      {
        if (jobCategories == null)
        {
          var jobCategoriesList = new JobCategory[]
          {
            new JobCategory{Name = "Pie research"},
            new JobCategory{Name = "Sales"},
            new JobCategory{Name = "Management"},
            new JobCategory{Name = "Store staff"},
            new JobCategory{Name = "Finance"},
            new JobCategory{Name = "QA"},
            new JobCategory{Name = "IT"},
            new JobCategory{Name = "Cleaning"},
            new JobCategory{Name = "Bakery"}
          };

          jobCategories = new Dictionary<string, JobCategory>();

          foreach (JobCategory jobCategory in jobCategoriesList)
          {
            jobCategories.Add(jobCategory.Name, jobCategory);
          }
        }

        return jobCategories;
      }
    }

    private static Dictionary<string, Country>? countries;

    public static Dictionary<string, Country> Countries
    {
      get
      {
        if (countries == null)
        {
          var countriesList = new Country[]
          {
            new Country {Name = "Belgium"},
            new Country {Name = "Netherlands"},
            new Country {Name = "USA"},
            new Country {Name = "Japan"},
            new Country {Name = "Canada"},
            new Country {Name = "UK"},
            new Country {Name = "France"},
            new Country {Name = "Brazil"}
          };

          countries = new Dictionary<string, Country>();

          foreach (Country country in countriesList)
          {
            countries.Add(country.Name, country);
          }
        }

        return countries;
      }
    }
  }
}
