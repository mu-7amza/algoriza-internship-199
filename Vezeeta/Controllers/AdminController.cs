using Core.Abstractions;
using Core.Dto;
using Core.Entities;
using Infrastructure.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

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
                    string folderPath = Path.Combine(wwwRootPath, @"images\doctors");

                    using (var stream = new FileStream(Path.Combine(folderPath, imageFileName), FileMode.Create))
                    {
                        doctorDto.file.CopyTo(stream);
                    }
                    doctorDto.ImageUrl = @"\images\doctors\" + imageFileName;
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

            var doctorFromDb = await _unitOfWork.ApplicationUser.GetAsync(u => u.Id == doctorId, includeProperities: "Specialization");

            if (doctorFromDb == null)
            {
                return NotFound("Doctor not found or may be deleted");
            }

            if (doctorDto.file != null)
            {
                string imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(doctorDto.file.FileName);
                string folderPath = Path.Combine(_env.WebRootPath, @"images\doctors");

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
                    doctorDto.ImageUrl = @"\images\doctors\" + imageFileName;
                }
            }


            // Create doctor, Specialization
            var doctorToModify = new ApplicationUser
            {
                FirstName = doctorDto.FirstName,
                LastName = doctorDto.LastName,
                UserName = doctorDto.Email,
                Email = doctorDto.Email,
                PhoneNumber = doctorDto.PhoneNumber,
                DateOfBirth = doctorDto.DateOfBirth.ToString(),
                ImageUrl = doctorDto.ImageUrl,
                Gender = doctorDto.Gender.ToString(),
            };

            var result = await _userManager.UpdateAsync(doctorToModify);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault());
            }


            if (doctorFromDb.Email != doctorDto.Email)
            {
                await _emailSender.SendEmailAsync(doctorDto.Email, "Your Personal Acccount Update ",
                    $"<h3>Your Email : {doctorDto.Email}</h3><br>" +
                    $"<h3>Your Password : {doctorDto.Password}</h3>");
            }


            if (doctorDto.SpecializationName != doctorFromDb.Specialization.Name)
            {

                _unitOfWork.Specialization.Update(doctorFromDb.Specialization);
                await _unitOfWork.SaveChangesAsync();
            }

            return Ok("Doctor added successfully");

        }
    }
}







