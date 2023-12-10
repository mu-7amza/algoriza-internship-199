using Core.Abstractions;
using Core.Dtos;
using Core.Entities;
using Infrastructure.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize (Roles = "Admin")]
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

        [HttpGet("Doctor/GetAll")]
        public async Task<IActionResult> GetAllDoctors(int page , int pagSize , string? search )
        {
          
            IEnumerable<ApplicationUser> doctorList = await _unitOfWork.ApplicationUser.GetAllPaginatedFilterAsync(
                filter: user => (string.IsNullOrEmpty(search) || user.FirstName.Contains(search)) && !string.IsNullOrEmpty(user.Specialization.Name),
                page: page,
                pageSize: pagSize,
                includeProperities: "Specialization");

            if (doctorList != null && doctorList.Any())
            {
                return Ok(doctorList); 
            }
            else
            {
                return NotFound("No Doctors in Database");
            }
        }

        [HttpGet("Doctor/GetById")]
        public async Task<IActionResult> GetDoctor(string Id)
        {
            ApplicationUser doctor = await _unitOfWork.ApplicationUser.GetAsync( u => u.Id == Id ,includeProperities: "Specialization");

            if (doctor != null)
            {
                return Ok(doctor);
            }
            else
            {
                return NotFound("Doctor not found in Database");
            }
        }

        [HttpPost("Doctor/Add")]
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

        [HttpPut("Doctor/Update")]
        public async Task<IActionResult> UpdateDoctor([FromForm] string doctorId, [FromForm] DoctorDto doctorDto)
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
                    doctorDto.ImageUrl = @"\Images\Doctors\" + imageFileName;
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

        [HttpDelete("Doctor/Delete")]
        public async Task<IActionResult> DeleteDoctor(string id)
        {

            var doctorFromDb = await _unitOfWork.ApplicationUser.GetAsync(u => u.Id == id, includeProperities: "Specialization", tracked: false);

            if (doctorFromDb == null)
            {
                return NotFound("Doctor not found");
            }

            if (!string.IsNullOrEmpty(doctorFromDb.ImageUrl))
            {
                var oldImagePath = Path.Combine(_env.WebRootPath,
                    doctorFromDb.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }


           var result =  await _userManager.DeleteAsync(doctorFromDb);
            _unitOfWork.Specialization.Delete(doctorFromDb.Specialization);
            await _unitOfWork.SaveChangesAsync();

            if(!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault());
            }

            return Ok("Doctor Deleted successfully");
        }

        [HttpGet("Patient/GetAll")]
        public async Task<IActionResult> GetAllPatients(int page, int pagSize, string? search)
        {

            IEnumerable<ApplicationUser> patientList = await _unitOfWork.ApplicationUser.GetAllPaginatedFilterAsync(
                filter: user => (string.IsNullOrEmpty(search) || user.FirstName.Contains(search)) && !string.IsNullOrEmpty(user.Booking.Status),
                page: page,
                pageSize: pagSize,
                includeProperities: "Booking");

            if (patientList != null && patientList.Any())
            {
                return Ok(patientList);
            }
            else
            {
                return NotFound("No Patient in Database");
            }
        }

        [HttpGet("Patient/GetById")]
        public async Task<IActionResult> GetPatient(string Id)
        {
            ApplicationUser doctor = await _unitOfWork.ApplicationUser.GetAsync(u => u.Id == Id, includeProperities: "Booking");

            if (doctor != null)
            {
                return Ok(doctor);
            }
            else
            {
                return NotFound("Doctor not found in Database");
            }
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

       

        [HttpPost("Discount/Add")]
        public async Task<IActionResult> AddDiscount([FromForm] DiscountDto discountDto)
        {

            if (discountDto == null)
            {
                return BadRequest("There is an error , try again");
            }

            if (ModelState.IsValid)
            {
                // Create Coupon, Discount

                Coupon coupon = new()
                {
                    Code = discountDto.Code,
                    Discount = new()
                    {
                        Name = discountDto.DiscountName,
                        DiscountType = discountDto.DiscountType.ToString(),
                        Value = (decimal) discountDto.Value
                    } 
                };

                await _unitOfWork.Coupon.AddAsync(coupon);
                await _unitOfWork.SaveChangesAsync();

            }
            return Ok("Coupon added successfully");
        }

        [HttpPut("Discount/Update")]
        public async Task<IActionResult> UpdateDiscount([FromForm] int couponId, [FromForm] DiscountDto discountDto)
        {

            var couponFromDb = await _unitOfWork.Coupon.GetAsync(u => u.Id == couponId, includeProperities: "Discount", tracked: false);

            if (couponFromDb == null)
            {
                return NotFound("Discount not found or may be deleted");
            }
            
            couponFromDb.Code = discountDto.Code;

             _unitOfWork.Coupon.Update(couponFromDb);
            await _unitOfWork.SaveChangesAsync();

            if (discountDto.DiscountName != null && couponFromDb.Discount != null)
            {
                Discount discount = await _unitOfWork.Discount.GetAsync(u => u.Id == couponFromDb.Discount.Id);

                discount.Name = discountDto.DiscountName;
                discount.DiscountType = discountDto.DiscountType.ToString();
                discount.Value = (decimal) discountDto.Value;
               
                await _unitOfWork.Discount.UpdateAsync(discount);
                await _unitOfWork.SaveChangesAsync();
            }

            return Ok("Coupon Updated successfully");

        }

        [HttpDelete("Discount/Delete")]
        public async Task<IActionResult> DeleteDiscount(int id)
        {

            Coupon couponFromDb = await _unitOfWork.Coupon.GetAsync(u => u.Id == id, includeProperities: "Discount", tracked: false);

            if (couponFromDb == null)
            {
                return NotFound("Coupon not found");
            }

            _unitOfWork.Coupon.Delete(couponFromDb);
            _unitOfWork.Discount.Delete(couponFromDb.Discount);
            await _unitOfWork.SaveChangesAsync();

            return Ok("Coupon Deleted successfully");
        }

        [HttpPut("Discount/Deactivate")]
        public async Task<IActionResult> DeactivateDiscount(int id)
        {
            Coupon couponFromDb = await _unitOfWork.Coupon.GetAsync(u => u.Id == id, includeProperities: "Discount", tracked: false);

            if (couponFromDb == null)
            {
                return NotFound("Coupon not found");
            }

            couponFromDb.IsValid = false;
             _unitOfWork.Coupon.Update(couponFromDb);
            await _unitOfWork.SaveChangesAsync();

            return Ok("Coupon Deactivated successfully");
        }

    }
}







