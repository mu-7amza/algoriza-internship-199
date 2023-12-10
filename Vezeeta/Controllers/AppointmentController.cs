using Core.Abstractions;
using Core.Dtos;
using Core.Entities;
using Infrastructure.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Doctor")]
    public class AppointmentController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public AppointmentController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllBookings")]
        public async Task<IActionResult> GetAll(int page, int pageSize, string? search)
        {
            ClaimsIdentity claims = (ClaimsIdentity)User.Identity;
            var currentDoctorEmail = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            var currentDoctor = await _userManager.FindByEmailAsync(currentDoctorEmail);

            IEnumerable<Booking> bookingList = await _unitOfWork.Booking.GetAllPaginatedFilterAsync(
               filter: booking => (string.IsNullOrEmpty(search) || booking.Status.Contains(search)) 
               || booking.Appointment.DoctorId == currentDoctor.Id,
               page: page,
               pageSize: pageSize,
               includeProperities: "Appointment");

            if (bookingList != null && bookingList.Any())
            {
                return Ok(bookingList);
            }
            else
            {
                return NotFound("No Bookings in Database");
            }
        }

        [HttpPost("AddAppointment")]
        public async Task<IActionResult> AddAppointment([FromForm] AppointmentDto appointmentDto)
        {
            if (appointmentDto == null)
            {
                return BadRequest("Error, Try again");
            }

            if (ModelState.IsValid)
            {
                ClaimsIdentity claims = (ClaimsIdentity)User.Identity;
                var doctorEmail = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
                var doctor = await _userManager.FindByEmailAsync(doctorEmail);

                var day = new Day { Date = appointmentDto.Day, Doctor = doctor };
                var time = new Time { TimeValue = appointmentDto.Time, Day = day };

                day.Times = new List<Time> { time };

                Appointment appointment = new()
                {
                    Doctor = doctor,
                    Price = appointmentDto.Price,
                };

                await _unitOfWork.Appointment.AddAsync(appointment);
                await _unitOfWork.SaveChangesAsync();
            }

            return Ok("Appointment Add Successfuly");
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAppointment([FromForm] int Id, [FromForm] AppointmentDto appointmentDto)
        {

            var appointmentFromDb = await _unitOfWork.Appointment.GetAsync(u => u.Id == Id, includeProperities: "Day", tracked: false);

            if (appointmentFromDb == null)
            {
                return NotFound("Appointment not found or may be deleted");
            }


            if (appointmentFromDb.Time.IsBooked == false)
            {
                appointmentFromDb.Price = appointmentDto.Price;
                appointmentFromDb.Time.TimeValue = appointmentDto.Time;
                appointmentFromDb.Day.Date = appointmentDto.Day;

                _unitOfWork.Appointment.Update(appointmentFromDb);
                _unitOfWork.SaveChangesAsync();

                return Ok("Appointment Updated successfully");
            }

            return Ok("Appointment can't be updated");
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAppointment([FromForm] int Id)
        {

            var appointmentFromDb = await _unitOfWork.Appointment.GetAsync(u => u.Id == Id);

            if (appointmentFromDb == null)
            {
                return NotFound("Appointment not found or may be deleted");
            }

            if (appointmentFromDb.Time.IsBooked == false)
            {
                _unitOfWork.Appointment.Delete(appointmentFromDb);
                _unitOfWork.SaveChangesAsync();

                return Ok("Appointment Deleted successfully");
            }
            return Ok("Appointment can't be updated");
        }


        [HttpPost("CheckUp")]
        public async Task<IActionResult> ConfirmCheckUp(int bookingId)
        {
            var booking = await _unitOfWork.Booking.GetAsync(u => u.Id == bookingId);

            if(booking == null)
            {
                return BadRequest("Not Found or Deleted");
            }

            booking.Status = SD.REQUEST_COMPLETED;

            _unitOfWork.Booking.Update(booking);
            await _unitOfWork.SaveChangesAsync();

            return Ok("CheckUp Confirmed");
        }
    }
}
