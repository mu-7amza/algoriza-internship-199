using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.JWT
{
    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; }

        [Required,MaxLength(256)]
        public string NewPassword { get; set; }

        [Required, MaxLength(256), Compare("NewPassword")]
        public string ConfirmnPassword { get; set; }
       
    }
}
