using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SerenityHospital.Business.Dtos.AdminstratorDtos;
using SerenityHospital.Business.Dtos.RoleDtos;
using SerenityHospital.Business.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SerenityHospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminstratorAuthsController : ControllerBase
    {
        readonly IAdminstratorService _service;

        public AdminstratorAuthsController(IAdminstratorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAllAsync());
        }

        [Authorize(Roles ="Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddRole([FromForm]AddRoleDto dto)
        {
            await _service.AddRoleAsync(dto);
            return Ok();

        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromForm]CreateAdminstratorDto dto)
        {
            await _service.CreateAsync(dto);
            return NoContent();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromForm] LoginAdminstratorDto dto)
        {
            return Ok(await _service.LoginAsync(dto));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> LoginWithRefreshToken(string refreshToken)
        {
            return Ok(await _service.LoginWithRefreshTokenAsync(refreshToken));
        }

        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> SoftDelete(string id)
        {
            await _service.SoftDeleteAsync(id);
            return NoContent();
        }

        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> ReverteSoftDelete(string id)
        {
            await _service.RevertSoftDeleteAsync(id);
            return NoContent();
        }
    }
}

