using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        //[Authorize(Roles = "Superadmin")]
        //[Authorize(Roles = "Admin")]
        //[Authorize(Roles = "Doctor")]
        //[Authorize(Roles = "Nurse")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAllAsync(true));
        }

        //[Authorize(Roles = "Superadmin")]
        //[Authorize(Roles = "Admin")]
        //[Authorize(Roles = "Doctor")]
        //[Authorize(Roles = "Nurse")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _service.GetByIdAsync(id,true));
        }

        //[Authorize(Roles = "Superadmin")]
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Post([FromForm]DoctorRoomCreateDro dto)
        {
            await _service.CreateAsync(dto);
            return StatusCode(StatusCodes.Status201Created);
        }


        //[Authorize(Roles = "Superadmin")]
        //[Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id,[FromForm] DoctorRoomUpdateDto dto)
        {
            await _service.UpdateAsync(id,dto);
            return NoContent();
        }


        //[Authorize(Roles = "Superadmin")]
        //[Authorize(Roles = "Admin")]
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

        //[Authorize(Roles = "Superadmin")]
        //[Authorize(Roles = "Admin")]
        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            await _service.SoftDeleteAsync(id);
            return NoContent();
        }


        //[Authorize(Roles = "Superadmin")]
        //[Authorize(Roles = "Admin")]
        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> ReverteSoftDelete(int id)
        {
            await _service.RevertSoftDeleteAsync(id);
            return NoContent();
        }
    }
}

