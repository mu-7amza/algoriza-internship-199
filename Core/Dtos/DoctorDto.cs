using Core.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Dto
{
    public class DoctorDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public string? Role { get; set; }
        [Required]
        public string DateOfBirth { get; set; }
        [Required]
        public Gender Gender { get; set; }

        public string? ImageUrl { get; set; }

        public IFormFile? file { get; set; }

        [Required]
        [RegularExpression(@"^(012|010|015|011)[0-9]{8}$", ErrorMessage = "Please enter a valid phone number.")]
        public string PhoneNumber { get; set; }

        public int? SpecializeId { get; set; }

        [Required(ErrorMessage = "Please select a specialization.")]
        public string SpecializationName { get; set; }




    }
}
