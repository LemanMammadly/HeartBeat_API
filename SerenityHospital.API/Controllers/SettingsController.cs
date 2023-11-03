using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SerenityHospital.Business.Dtos.SettingDtos;
using SerenityHospital.Business.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SerenityHospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        readonly ISettingService _service;

        public SettingsController(ISettingService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAllAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Post([FromForm]SettingCreateDto dto)
        {
            await _service.CreateAsync(dto);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Put(int id,[FromForm]SettingUpdateDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok();
        }
    }
}

