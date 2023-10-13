using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SerenityHospital.Business.Dtos.DoctorRoom;
using SerenityHospital.Business.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SerenityHospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorRoomsController : ControllerBase
    {
        readonly IDoctorRoomService _service;

        public DoctorRoomsController(IDoctorRoomService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAllAsync(true));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _service.GetByIdAsync(id,true));
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromForm]DoctorRoomCreateDro dto)
        {
            await _service.CreateAsync(dto);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id,[FromForm] DoctorRoomUpdateDto dto)
        {
            await _service.UpdateAsync(id,dto);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            await _service.SoftDeleteAsync(id);
            return NoContent();
        }

        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> ReverteSoftDelete(int id)
        {
            await _service.RevertSoftDeleteAsync(id);
            return NoContent();
        }
    }
}

