using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SerenityHospital.Business.Dtos.AdminDtos;
using SerenityHospital.Business.Dtos.AdminstratorDtos;
using SerenityHospital.Business.Dtos.RoleDtos;
using SerenityHospital.Business.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SerenityHospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAuthsController : ControllerBase
    {
        readonly IAdminService _service;

        public AdminAuthsController(IAdminService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAllAsync());
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _service.GetById(id));
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromForm]AdminCreateDto dto)
        {
            await _service.CreateAsync(dto);
            return StatusCode(StatusCodes.Status201Created);
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> AddRole([FromForm] AddRoleDto dto)
        {
            await _service.AddRoleAsync(dto);
            return Ok();
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> RemoveRole([FromForm] RemoveRoleDto dto)
        {
            await _service.RemoveRoleAsync(dto);
            return Ok();
        }


        [HttpPut("[action]")]
        public async Task<IActionResult> Put([FromForm] AdminUpdateDto dto)
        {
            await _service.UpdateAsync(dto);
            return NoContent();
        }


        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> PutByAdmin(string id, [FromForm] AdminUpdateBySuperAdminDto dto)
        {
            await _service.UpdateByAdminAsync(id, dto);
            return NoContent();
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromForm] AdminLoginDto dto)
        {
            return Ok(await _service.LoginAsync(dto));
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> LoginWithRefreshToken(string refreshToken)
        {
            return Ok(await _service.LoginWithRefreshTokenAsync(refreshToken));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Logout()
        {
            await _service.Logout();
            return NoContent();
        }
    }
}

