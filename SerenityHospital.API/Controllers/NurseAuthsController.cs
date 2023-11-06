using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SerenityHospital.Business.Dtos.NurseDtos;
using SerenityHospital.Business.Dtos.RoleDtos;
using SerenityHospital.Business.ExternalServices.Interfaces;
using SerenityHospital.Business.Services.Interfaces;
using SerenityHospital.Core.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SerenityHospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NurseAuthsController : ControllerBase
    {
        readonly INurseService _service;
        readonly UserManager<Nurse> _userManager;
        readonly IEmailServiceSender _emailService;

        public NurseAuthsController(INurseService service, UserManager<Nurse> userManager, IEmailServiceSender emailService)
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
            return Ok(await _service.GetById(true,id));
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetByName(string username)
        {
            return Ok(await _service.GetByName(username));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Count()
        {
            return Ok(await _service.Count());
        }


        [Authorize(Roles = "Receptionist")]
        [HttpPut("[action]")]
        public async Task<IActionResult> Put([FromForm]NurseUpdateDto dto)
        {
            await _service.UpdateAsync(dto);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> PutByAdmin(string id,[FromForm] NurseUpdateByAdminDto dto)
        {
            await _service.UpdateByAdminAsync(id,dto);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromForm]NurseCreateDto dto)
        {
            await _service.CreateAsync(dto);
            return StatusCode(StatusCodes.Status201Created);
            //var user = await _userManager.FindByEmailAsync(dto.Email);
            //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var confirmationLink = Url.Action("ConfirmEmail", "NurseAuths", new { token, email = dto.Email }, Request.Scheme);
            //var message = new Message(new string[] { dto.Email! }, "Confirmation email link", confirmationLink!);
            //_emailService.SendEmail(message);
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

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromForm] NurseLoginDto dto)
        {
            //var user = await _userManager.FindByNameAsync(dto.UserName);

            //if (user.EmailConfirmed == false)
            //{
            //    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //    var confirmationLink = Url.Action("ConfirmEmail", "NurseAuths", new { token, email = user.Email }, Request.Scheme);
            //    var message = new Message(new string[] { user.Email! }, "Confirmation email link", confirmationLink!);
            //    _emailService.SendEmail(message);
            //    return StatusCode(StatusCodes.Status201Created);
            //}
            return Ok(await _service.LoginAsync(dto));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> LoginWithRefreshToken(string refreshToken)
        {
            return Ok(await _service.LoginWithRefreshTokenAsync(refreshToken));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddRole([FromForm]AddRoleDto dto)
        {
            await _service.AddRole(dto);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> RemoveRole([FromForm] RemoveRoleDto dto)
        {
            await _service.RemoveRole(dto);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Receptionist")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Logout()
        {
            await _service.Logout();
            return NoContent();
        }
    }
}

