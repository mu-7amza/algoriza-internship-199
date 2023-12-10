using Core.Abstractions;
using Core.Entities;
using Infrastructure.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize(Roles = "Patient")]
    public class BookingController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public BookingController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("Patient/GetAllDoctors")]
        public async Task<IActionResult> GetAll(int page , int pageSize , string? search)
        {
            ClaimsIdentity claims = (ClaimsIdentity)User.Identity;
            var pateintEmail = claims.FindFirst(ClaimTypes.NameIdentifier).Value; 
            
            var patient = await _userManager.FindByEmailAsync(pateintEmail);

            IEnumerable<Booking> bookingList = await _unitOfWork.Booking.GetAllPaginatedFilterAsync(
               filter: booking => (string.IsNullOrEmpty(search) || booking.Appointment.Doctor.FirstName.Contains(search)),
               page: page,
               pageSize: pageSize,
               includeProperities: "Appointment");

            if (bookingList != null && bookingList.Any())
            {
                return Ok(bookingList);
            }
            else
            {
                return NotFound("No Doctors in Database");
            }
        }

        [HttpPost("Patient/Book")]
        public async Task<IActionResult> Book(int id)
        {

            ClaimsIdentity claims = (ClaimsIdentity)User.Identity;
            var pateintEmail = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            var patient = await _userManager.FindByEmailAsync(pateintEmail);

            if (patient == null)
            {
                return NotFound("Error , Try again");
            }

            IEnumerable<Booking> patientBooking = await _unitOfWork.Booking.GetAllPaginatedFilterAsync(u => u.Id == patient.Booking.Id);
            IEnumerable<Coupon> coupons = await _unitOfWork.Coupon.GetAllPaginatedFilterAsync(includeProperities: "Discount");

            var appointment = await _unitOfWork.Appointment.GetAsync(u => u.Id == id);

            var result = await ApplyCoupon(patientBooking , coupons);

            if (result != null)
            {
                appointment.Price -= result.Discount.Value;
            }
            patient.Booking = new()
            {
                Status = SD.REQUEST_PENDING,
                Appointment = appointment,
                DoctorDay = appointment.Day,
                DoctorTime = appointment.Time,
                TimeId = appointment.Time.Id
            };

             await _unitOfWork.Booking.AddAsync(patient.Booking);
            await _unitOfWork.SaveChangesAsync();

            return Ok("Booking is Proceeded Successufuly");

        }

        [HttpGet("Patient/Bookings/GetAll")]
        public async Task<IActionResult> GetAllBookings()
        {
            ClaimsIdentity claims = (ClaimsIdentity)User.Identity;
            var pateintEmail = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            var patient = await _userManager.FindByEmailAsync(pateintEmail);

            IEnumerable<Booking> bookingList = await _unitOfWork.Booking.GetAllPaginatedFilterAsync(
               u => u.Id == patient.BookingId,
               includeProperities: "Appointment");

            if (bookingList != null && bookingList.Any())
            {
                return Ok(bookingList);
            }
            else
            {
                return NotFound("No Doctors in Database");
            }
        }


        [HttpPost("Patient/Booking/Cancel")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var Booking = await _unitOfWork.Booking.GetAsync( u => u.Id == id);
            if (Booking != null)
            {
                Booking.Status = SD.REQUEST_Canceled;
                _unitOfWork.Booking.Update(Booking);
                await _unitOfWork.SaveChangesAsync();

                return Ok($"Booking {id} is canceled");
            }

            return BadRequest("Error , please try again");
            
        }

        public static async Task<Coupon> ApplyCoupon(IEnumerable<Booking> patientBookings, IEnumerable<Coupon> coupons)
        {
            if (patientBookings.Count() >= 5)
            {
                var coupon = coupons.FirstOrDefault(u => u.IsValid == true);
                return coupon;
            }
            return null;

        }
    }
}
