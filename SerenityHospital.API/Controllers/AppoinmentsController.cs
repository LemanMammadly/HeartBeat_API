using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SerenityHospital.Business.Dtos.AppoinmentDtos;
using SerenityHospital.Business.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SerenityHospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppoinmentsController : ControllerBase
    {
        readonly IAppoinmentService _service;

        public AppoinmentsController(IAppoinmentService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin,Doctor,Patient,Receptionist")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAllAsync(true));
        }

        [Authorize(Roles = "Admin,Doctor,Patient,Receptionist")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        { 
            return Ok(await _service.GetByIdAsync(id,true));
        }

        [Authorize(Roles = "Doctor,Patient,Receptionist")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetByUsername(string userName)
        {
            return Ok(await _service.GetByUsernameAsync(userName));
        }


        [Authorize(Roles = "Admin,Receptionist")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm]AppoinmentUpdateDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [Authorize(Roles = "Doctor,Patient")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]AppoinmentCreateDto dto)
        {
            await _service.CreateAsync(dto);
            return StatusCode(StatusCodes.Status201Created);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost("[action]")]
        public async Task<IActionResult> AcceptAppoinment(int id)
        {
            await _service.AcceptAppoinment(id);
            return NoContent();
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost("[action]")]
        public async Task<IActionResult> RejectAppoinment(int id)
        {
            await _service.RejectAppoinment(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin,Receptionist,Patient,Doctor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> Count()
        {
            return Ok(await _service.Count());
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            await _service.SoftDeleteAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> ReverteSoftDelete(int id)
        {
            await _service.ReverteSoftDeleteAsync(id);
            return NoContent();
        }
    }
}

