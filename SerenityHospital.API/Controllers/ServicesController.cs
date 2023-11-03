using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SerenityHospital.Business.Dtos.ServiceDtos;
using SerenityHospital.Business.ExternalServices.Interfaces;
using SerenityHospital.Business.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SerenityHospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        readonly IServiceService _service;
        readonly ITokenService _tokenService;

        public ServicesController(IServiceService service, ITokenService tokenService)
        {
            _service = service;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAllAsync(true));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _service.GetByIdAsync(id, true));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ServiceCreateDto dto)
        {
            await _service.CreateAsync(dto);
            return NoContent();
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> Count()
        {
            return Ok(await _service.Count());
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Put(int id,[FromBody]ServiceUpdateDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            await _service.SoftDeleteAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> ReverteSoftDelete(int id)
        {
            await _service.ReverteSoftDeleteAsync(id);
            return NoContent();
        }
    }
}

