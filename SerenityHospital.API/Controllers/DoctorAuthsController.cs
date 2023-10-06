using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SerenityHospital.Business.Dtos.DoctorDtos;
using SerenityHospital.Business.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SerenityHospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorAuthsController : ControllerBase
    {
        readonly IDoctorService _service;

        public DoctorAuthsController(IDoctorService service)
        {
            _service = service;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromForm] DoctorCreateDto dto)
        {
            await _service.CreateAsync(dto);
            return StatusCode(StatusCodes.Status201Created);
        }

    }
}

