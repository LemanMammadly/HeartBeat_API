using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SerenityHospital.Business.Dtos.NurseDtos;
using SerenityHospital.Business.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SerenityHospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NurseAuthsController : ControllerBase
    {
        readonly INurseService _service;

        public NurseAuthsController(INurseService service)
        {
            _service = service;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromForm]NurseCreateDto dto)
        {
            await _service.CreateAsync(dto);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromForm] NurseLoginDto dto)
        {
            return Ok(await _service.LoginAsync(dto));   
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> LoginWithRefreshToken(string refreshToken)
        {
            return Ok(await _service.LoginWithRefreshTokenAsync(refreshToken));
        }
    }
}

