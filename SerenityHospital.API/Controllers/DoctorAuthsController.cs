using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SerenityHospital.Business.Dtos.DoctorDtos;
using SerenityHospital.Business.Dtos.RoleDtos;
using SerenityHospital.Business.ExternalServices.Interfaces;
using SerenityHospital.Business.Services.Interfaces;
using SerenityHospital.Core.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SerenityHospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorAuthsController : ControllerBase
    {
        readonly IDoctorService _service;
        readonly UserManager<Doctor> _userManager;
        readonly IEmailServiceSender _emailService;

        public DoctorAuthsController(IDoctorService service, UserManager<Doctor> userManager, IEmailServiceSender emailService)
        {
            _service = service;
            _userManager = userManager;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAllAsync(true));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _service.GetById(id,true));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Count()
        {
            return Ok(await _service.Count());
        }



        //[Authorize(Roles = "Superadmin")]
        //[Authorize(Roles = "Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromForm] DoctorCreateDto dto)
        {
            await _service.CreateAsync(dto);
            var user = await _userManager.FindByEmailAsync(dto.Email);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail", "DoctorAuths", new { token, email = dto.Email }, Request.Scheme);
            var message = new Message(new string[] { dto.Email! }, "Confirmation email link", confirmationLink!);
            _emailService.SendEmail(message);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK);
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromForm]DoctorLoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);

            if (user.EmailConfirmed == false)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "DoctorAuths", new { token, email = user.Email }, Request.Scheme);
                var message = new Message(new string[] { user.Email! }, "Confirmation email link", confirmationLink!);
                _emailService.SendEmail(message);
                return StatusCode(StatusCodes.Status201Created);
            }
            return Ok(await _service.LoginAsync(dto));
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost("[action]")]
        public async Task<IActionResult> LoginWithRefreshToken(string refreshToken)
        {
            return Ok(await _service.LoginWithRefreshTokenAsync(refreshToken));
        }

        [Authorize(Roles = "Superadmin")]
        [Authorize(Roles = "Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddRole([FromForm]AddRoleDto dto)
        {
            await _service.AddRole(dto);
            return Ok();
        }

        [Authorize(Roles = "Superadmin")]
        [Authorize(Roles = "Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> RemoveRole([FromForm] RemoveRoleDto dto)
        {
            await _service.RemoveRole(dto);
            return Ok();
        }

        [Authorize(Roles = "Superadmin")]
        [Authorize(Roles = "Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddDoctorRoom([FromForm] AddDoctorRoomDto dto)
        {
            await _service.AddDoctorRoom(dto);
            return Ok();
        }

        [Authorize(Roles = "Doctor")]
        [HttpPut("[action]")]
        public async Task<IActionResult> Put([FromForm] DoctorUpdateDto dto)
        {
            await _service.UpdateAsync(dto);
            return NoContent();
        }

        [Authorize(Roles = "Superadmin")]
        [Authorize(Roles = "Admin")]
        [HttpPut("[action]")]
        public async Task<IActionResult> PutByAdmin(string id,[FromForm] DoctorUpdateByAdminDto dto)
        {
            await _service.UpdateByAdminAsync(id,dto);
            return NoContent();
        }

        [Authorize(Roles = "Superadmin")]
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Logout()
        {
            await _service.Logout();
            return NoContent();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Updater(string id)
        {
            await _service.DoctorStatusUpdater(id);
            return Ok();
        }
    }
}

