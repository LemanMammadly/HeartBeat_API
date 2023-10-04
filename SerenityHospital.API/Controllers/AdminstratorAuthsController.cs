using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SerenityHospital.Business.Dtos.AdminstratorDtos;
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

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromForm]CreateAdminstratorDto dto)
        {
            await _service.CreateAsync(dto);
            return NoContent();
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

