using System.ComponentModel.DataAnnotations;

namespace ClientApp.Models.Dtos
{
  public class EmployeeDto
  {
    public int EmployeeId { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "First name is too long.")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "Last name is too long.")]
    public string LastName { get; set; }

    public DateTime? BirthDate { get; set; }

    [Required]
    public string Email { get; set; }
    public string? Street { get; set; } = string.Empty;
    public string? Zip { get; set; } = string.Empty;
    public string? City { get; set; } = string.Empty;
    public int CountryId { get; set; }
    public CountryDto? Country { get; set; } = new CountryDto();
    public string? PhoneNumber { get; set; } = string.Empty;
    public bool Smoker { get; set; }
    public MaritalStatusDto MaritalStatus { get; set; }
    public GenderDto Gender { get; set; }

    [StringLength(1000, ErrorMessage = "Comment length can't exceed 1000 characters.")]
    public string? Comment { get; set; } = string.Empty;

    public DateTime? JoinedDate { get; set; }
    public DateTime? ExitDate { get; set; }

    public int JobCategoryId { get; set; }
    public JobCategoryDto? JobCategory { get; set; } = new JobCategoryDto();

    public double Latitude { get; set; }
    public double Longitude { get; set; }
  }
}
