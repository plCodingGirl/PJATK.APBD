using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Models;

namespace Pharmacy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly PharmacyDbContext _context;

        public DoctorsController(PharmacyDbContext context)
        {
            _context = context;
        }

        // GET: api/Doctors
        [HttpGet]
        public async Task<IActionResult> GetDoctors()
        {
            return Ok(await _context.Doctors.ToListAsync());
        }

        // GET: api/Doctors/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditDoctor(int id, DoctorDto doctorDto)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            doctor.FirstName = doctorDto.FirstName;
            doctor.LastName = doctorDto.LastName;
            doctor.Email = doctorDto.Email;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDoctor(DoctorDto doctorDto)
        {
            var doctor = new Doctor
            {
                FirstName = doctorDto.FirstName, 
                LastName = doctorDto.LastName, 
                Email = doctorDto.Email
            };

            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();

            return Created("", doctor);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
