using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Core.Enums;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace Core.Dtos.JWT
{
    public class RegisterDto
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [Required, MaxLength(50)]
        public string UserName { get; set; }

        [Required, MaxLength(128)]
        [EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(256)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, MaxLength(256),Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmedPassword { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [JsonIgnore]
        public string? ImageUrl { get; set; }

        public IFormFile? FormFile { get; set; }

        [Required]
        public string DateOfBirth { get; set; }

        [Required]
        [RegularExpression(@"^(012|010|015|011)[0-9]{8}$", ErrorMessage = "Please enter a valid phone number.")]
        public string PhoneNumber { get; set; }



    }
}
