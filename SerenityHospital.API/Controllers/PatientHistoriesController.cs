﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SerenityHospital.Business.Services.Interfaces;
using SerenityHospital.Core.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SerenityHospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientHistoriesController : ControllerBase
    {
        readonly IPatientHistoryService _service;

        public PatientHistoriesController(IPatientHistoryService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin,Doctor,Patient")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAllAsync());
        }


        [Authorize(Roles = "Admin,Doctor,Patient")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [Authorize(Roles = "Admin,Doctor,Patient")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetByName(string username)
        {
            return Ok(await _service.GetByNameAsync(username));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Count()
        {
            return Ok(await _service.Count());
        }

    }
}

