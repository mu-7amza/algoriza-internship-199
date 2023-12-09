using Core.Abstractions;
using Core.Dto;
using Core.Entities;
using Infrastructure.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        public AdminController(UserManager<ApplicationUser> userManager, IWebHostEnvironment env, IEmailSender emailSender, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _env = env;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("AddDoctor")]
        public async Task<IActionResult> AddDoctor([FromForm] DoctorDto doctorDto)
        {
            if (doctorDto == null)
            {
                return BadRequest("There is an error , try again");
            }

            if (ModelState.IsValid)
            {
                string wwwRootPath = _env.WebRootPath;
                if (doctorDto.file != null)
                {
                    string imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(doctorDto.file.FileName);
                    string folderPath = Path.Combine(wwwRootPath, @"Images\Doctors");

                    using (var stream = new FileStream(Path.Combine(folderPath, imageFileName), FileMode.Create))
                    {
                        doctorDto.file.CopyTo(stream);
                    }
                    doctorDto.ImageUrl = @"\Images\Doctors\" + imageFileName;
                }


                // Create doctor, Specialization
                var doctorToAdd = new ApplicationUser
                {
                    FirstName = doctorDto.FirstName,
                    LastName = doctorDto.LastName,
                    UserName = doctorDto.Email,
                    Email = doctorDto.Email,
                    PhoneNumber = doctorDto.PhoneNumber,
                    DateOfBirth = doctorDto.DateOfBirth.ToString(),
                    ImageUrl = doctorDto.ImageUrl,
                    Gender = doctorDto.Gender.ToString(),
                    Specialization = new Specialization()
                    {
                        Name = doctorDto.SpecializationName
                    }
                };

                await _unitOfWork.Specialization.AddAsync(doctorToAdd.Specialization);
                await _unitOfWork.SaveChangesAsync();

                if(doctorDto.Password == null)
                {
                    doctorDto.Password = GenerateStrongPassword(15);
                }
                var result = await _userManager.CreateAsync(doctorToAdd, doctorDto.Password);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors.FirstOrDefault());
                }

                // Add Rule to db and send credintials to doctor mail

                await _userManager.AddToRoleAsync(doctorToAdd, SD.ROLE_DOCTOR);
                await _emailSender.SendEmailAsync(doctorDto.Email, "Your Personal Acccount",
                    $"<h3>Your Email : {doctorDto.Email}</h3><br>" +
                    $"<h3>Your Password : {doctorDto.Password}</h3>");

                // Store Specialization along with Doctor Id


            }
            return Ok("Doctor added successfully");
        }

        [HttpPut("EditDoctor")]
        public async Task<IActionResult> EditDoctor([FromForm] string doctorId, [FromForm] DoctorDto doctorDto)
        {

            var doctorFromDb = await _unitOfWork.ApplicationUser.GetAsync(u => u.Id == doctorId, includeProperities: "Specialization",tracked:false);

            if (doctorFromDb == null)
            {
                return NotFound("Doctor not found or may be deleted");
            }

            if (doctorDto.file != null)
            {
                string imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(doctorDto.file.FileName);
                string folderPath = Path.Combine(_env.WebRootPath, @"Images\Doctors");

                if (doctorDto.file != null)
                {
                    if (!string.IsNullOrEmpty(doctorFromDb.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_env.WebRootPath,
                            doctorFromDb.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var stream = new FileStream(Path.Combine(folderPath, imageFileName), FileMode.Create))
                    {
                        doctorDto.file.CopyTo(stream);
                    }
                    doctorDto.ImageUrl = @"\Images\Doctors" + imageFileName;
                }
            }

            
            doctorFromDb.FirstName = doctorDto.FirstName;
            doctorFromDb.LastName = doctorDto.LastName;
            doctorFromDb.Email = doctorDto.Email;
            doctorFromDb.Gender = doctorDto.Gender.ToString();
            doctorFromDb.DateOfBirth = doctorDto.DateOfBirth.ToString();
            doctorFromDb.PhoneNumber = doctorDto.PhoneNumber;
            doctorFromDb.ImageUrl = doctorDto.ImageUrl;
            doctorFromDb.UserName = doctorDto.Email;
            doctorFromDb.NormalizedEmail = doctorDto.Email.ToUpper();
            doctorFromDb.NormalizedUserName = doctorDto.Email.ToUpper();

            _unitOfWork.ApplicationUser.Update(doctorFromDb);
            await _unitOfWork.SaveChangesAsync();
            
                await _emailSender.SendEmailAsync(doctorDto.Email, "Your Personal Acccount Update ",
                   $"<h3>Your Email : {doctorDto.Email}</h3><br>" +
                   $"<h3>Your Password : {doctorDto.Password}</h3>");

                if (doctorDto.SpecializationName != null && doctorFromDb.Specialization != null)
                {
                    Specialization specialization = new()
                    {
                        Id = doctorFromDb.Specialization.Id,
                        Name = doctorDto.SpecializationName

                    };
                    await _unitOfWork.Specialization.Update(specialization);
                    await _unitOfWork.SaveChangesAsync();
                }
               
            return Ok("Doctor Updated successfully");

        }

        public static string GenerateStrongPassword(int length)
        {
            const string validCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";

            // Use RNGCryptoServiceProvider for better randomness
            using (var rng = new RNGCryptoServiceProvider())
            {
                // Generate a random byte array
                byte[] randomBytes = new byte[length];
                rng.GetBytes(randomBytes);

                // Convert the random bytes to characters from the valid character set
                StringBuilder passwordBuilder = new StringBuilder(length);
                foreach (byte b in randomBytes)
                {
                    passwordBuilder.Append(validCharacters[b % validCharacters.Length]);
                }

                return passwordBuilder.ToString();
            }
        }
    }
}







