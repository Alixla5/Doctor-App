using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DoctorApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly DoctorContext _context;

        public DoctorsController(DoctorContext context)
        {
            _context = context;
        }

        // GET /Doctors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors([FromQuery] string docType)
        {
            IQueryable<Doctor> doctorsQuery = _context.Doctors;

            if (!string.IsNullOrEmpty(docType))
            {
                doctorsQuery = doctorsQuery.Where(d => d.Type == docType);
            }

            var doctors = await doctorsQuery.ToListAsync();
            return Ok(doctors);
        }

        // GET /Doctors/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return doctor;
        }

        // POST /Doctors
        [HttpPost]
        public async Task<ActionResult<Doctor>> PostDoctor(Doctor newDoctor)
        {
            _context.Doctors.Add(newDoctor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDoctor), new { id = newDoctor.Id }, newDoctor);
        }

        // PUT /Doctors/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDoctor(int id, Doctor doctor)
        {
            if (id != doctor.Id)
            {
                return BadRequest();
            }

            _context.Entry(doctor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE /Doctors/{id}
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

            return NoContent();
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.Id == id);
        }
    }
}
