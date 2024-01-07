using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Db;
using Api.Models;

namespace Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BloodRequestDonationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BloodRequestDonationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BloodRequestDonations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BloodRequestDonation>>> GetBloodRequestDonation()
        {
            return await _context.BloodRequestDonation.ToListAsync();
        }

        // GET: api/BloodRequestDonations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BloodRequestDonation>> GetBloodRequestDonation(int id)
        {
            var bloodRequestDonation = await _context.BloodRequestDonation.FindAsync(id);

            if (bloodRequestDonation == null)
            {
                return NotFound();
            }

            return bloodRequestDonation;
        }

        // PUT: api/BloodRequestDonations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBloodRequestDonation(int id, BloodRequestDonation bloodRequestDonation)
        {
            if (id != bloodRequestDonation.BloodRequestId)
            {
                return BadRequest();
            }

            _context.Entry(bloodRequestDonation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BloodRequestDonationExists(id))
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

        // POST: api/BloodRequestDonations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BloodRequestDonation>> PostBloodRequestDonation(BloodRequestDonation bloodRequestDonation)
        {
            _context.BloodRequestDonation.Add(bloodRequestDonation);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BloodRequestDonationExists(bloodRequestDonation.BloodRequestId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBloodRequestDonation", new { id = bloodRequestDonation.BloodRequestId }, bloodRequestDonation);
        }

        // DELETE: api/BloodRequestDonations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBloodRequestDonation(int id)
        {
            var bloodRequestDonation = await _context.BloodRequestDonation.FindAsync(id);
            if (bloodRequestDonation == null)
            {
                return NotFound();
            }

            _context.BloodRequestDonation.Remove(bloodRequestDonation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BloodRequestDonationExists(int id)
        {
            return _context.BloodRequestDonation.Any(e => e.BloodRequestId == id);
        }
    }
}
