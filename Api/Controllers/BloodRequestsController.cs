using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Db;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Api.Services;

namespace Api.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class BloodRequestsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly SendgridEmailSender _sendgridEmailSender;

		public BloodRequestsController(ApplicationDbContext context, SendgridEmailSender sendgridEmailSender)
		{
			_context = context;
			_sendgridEmailSender = sendgridEmailSender;

		}

		// GET: api/BloodRequests
		[HttpGet]
		[Authorize]
		public async Task<ActionResult<IEnumerable<BloodRequest>>> GetBloodRequests()
		{
			return await _context.BloodRequests.ToListAsync();
		}

		// GET: api/BloodRequests/5
		[HttpGet("{id}")]
		[Authorize]
		public async Task<ActionResult<BloodRequest>> GetBloodRequest(int id)
		{
			var bloodRequest = await _context.BloodRequests.FindAsync(id);

			if (bloodRequest == null)
			{
				return NotFound();
			}

			return bloodRequest;
		}

		// PUT: api/BloodRequests/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		[Authorize]
		public async Task<IActionResult> PutBloodRequest(int id, BloodRequest bloodRequest)
		{
			if (id != bloodRequest.Id)
			{
				return BadRequest();
			}

			_context.Entry(bloodRequest).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!BloodRequestExists(id))
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

		// POST: api/BloodRequests
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		[Authorize]
		public async Task<ActionResult<BloodRequest>> PostBloodRequest(BloodRequest bloodRequest)
		{
			_context.BloodRequests.Add(bloodRequest);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetBloodRequest", new { id = bloodRequest.Id }, bloodRequest);
		}

		// DELETE: api/BloodRequests/5
		[HttpDelete("{id}")]
		[Authorize]
		public async Task<IActionResult> DeleteBloodRequest(int id)
		{
			var bloodRequest = await _context.BloodRequests.FindAsync(id);
			if (bloodRequest == null)
			{
				return NotFound();
			}

			_context.BloodRequests.Remove(bloodRequest);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool BloodRequestExists(int id)
		{
			return _context.BloodRequests.Any(e => e.Id == id);
		}


		[HttpPost("check/{id}")]
		//[Authorize]
		public async Task<ActionResult<bool>> CheckRequest(int id)
		{
			var request = await _context.BloodRequests.FirstOrDefaultAsync(e => e.Id == id);
			if (request == null) 
				return NotFound();

			await _sendgridEmailSender.SendEmailAsync(request.Email, "Your blood request", "test");

			return Ok(true);
		}

	}
}
