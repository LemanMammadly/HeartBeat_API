using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SerenityHospital.Business.Dtos.PatientDtos;
using SerenityHospital.Business.Dtos.RoleDtos;
using SerenityHospital.Business.ExternalServices.Interfaces;
using SerenityHospital.Business.Services.Interfaces;
using SerenityHospital.Core.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SerenityHospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientAuthsController : ControllerBase
    {
        readonly IPatientService _service;
        readonly UserManager<Patient> _userManager;
        readonly IEmailServiceSender _emailService;

        public PatientAuthsController(IPatientService service, UserManager<Patient> userManager, IEmailServiceSender emailService)
        {
            _service = service;
            _userManager = userManager;
            _emailService = emailService;
        }

        [Authorize(Roles = "Superadmin")]
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Doctor")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAllAsync());
        }


        [Authorize(Roles = "Superadmin")]
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Doctor")]
        [Authorize(Roles = "Patient")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _service.GetById(id));
        }

        [Authorize(Roles = "Superadmin")]
        [Authorize(Roles = "Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromForm]PatientCreateDto dto)
        {
            await _service.CreateAsync(dto);
            //var user = await _userManager.FindByNameAsync(dto.UserName);
            //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var confirmationLink = Url.Action("ConfirmEmail", "PatientAuths", new { token, email = user.Email }, Request.Scheme);
            //var message = new Message(new string[] { user.Email! }, "Confirmation email link", confirmationLink!);
            //_emailService.SendEmail(message);
            return StatusCode(StatusCodes.Status201Created);
        }

        //[HttpGet("ConfirmEmail")]
        //public async Task<IActionResult> ConfirmEmail(string token, string email)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user != null)
        //    {
        //        var result = await _userManager.ConfirmEmailAsync(user, token);
        //        if (result.Succeeded)
        //        {
        //            return StatusCode(StatusCodes.Status200OK);
        //        }
        //    }
        //    return StatusCode(StatusCodes.Status500InternalServerError);
        //}

        [Authorize(Roles = "Patient")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromForm]PatientLoginDto dto)
        {
            //var user = await _userManager.FindByNameAsync(dto.UserName);

            //if (user.EmailConfirmed == false)
            //{
            //    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //    var confirmationLink = Url.Action("ConfirmEmail", "PatientAuths", new { token, email = user.Email }, Request.Scheme);
            //    var message = new Message(new string[] { user.Email! }, "Confirmation email link", confirmationLink!);
            //    _emailService.SendEmail(message);
            //    return StatusCode(StatusCodes.Status201Created);
            //}
            return Ok(await _service.LoginAsync(dto));
        }

        [Authorize(Roles = "Patient")]
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
            return NoContent();
        }

        [Authorize(Roles = "Superadmin")]
        [Authorize(Roles = "Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> RemoveRole([FromForm] RemoveRoleDto dto)
        {
            await _service.RemoveRole(dto);
            return NoContent();
        }

        [Authorize(Roles = "Patient")]
        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromForm] PatientUpdateDto dto)
        {
            await _service.UpdateAsync(dto);
            return NoContent();
        }

        [Authorize(Roles = "Superadmin")]
        [Authorize(Roles = "Admin")]
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateByAdmin(string id,[FromForm] PatientUpdateByAdminDto dto)
        {
            await _service.UpdateByAdminAsync(id,dto);
            return NoContent();
        }


        [Authorize(Roles = "Superadmin")]
        [Authorize(Roles = "Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddPatientRoom([FromForm] AddPatientRoomDto dto)
        {
            await _service.AddPatientRoom(dto);
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

        [Authorize(Roles = "Patient")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Logout()
        {
            await _service.Logout();
            return NoContent();
        }
    }
}

