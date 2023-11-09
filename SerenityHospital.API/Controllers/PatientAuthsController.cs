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

        [Authorize(Roles = "Admin,Doctor")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAllAsync());
        }


        [Authorize(Roles = "Admin,Doctor,Patient")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _service.GetById(id));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetByName(string userName)
        {
            return Ok(await _service.GetByName(userName));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromForm]PatientCreateDto dto)
        {
            await _service.CreateAsync(dto);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromForm]PatientLoginDto dto)
        {
            return Ok(await _service.LoginAsync(dto));
        }

        //[Authorize(Roles = "Patient")]
        [HttpPost("[action]")]
        public async Task<IActionResult> LoginWithRefreshToken(string refreshToken)
        {
            return Ok(await _service.LoginWithRefreshTokenAsync(refreshToken));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Count()
        {
            return Ok(await _service.Count());
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddRole([FromForm]AddRoleDto dto)
        {
            await _service.AddRole(dto);
            return NoContent();
        }

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

        [Authorize(Roles = "Admin")]
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateByAdmin(string id,[FromForm] PatientUpdateByAdminDto dto)
        {
            await _service.UpdateByAdminAsync(id,dto);
            return NoContent();
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddPatientRoom([FromForm] AddPatientRoomDto dto)
        {
            await _service.AddPatientRoom(dto);
            return NoContent();
        }

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

