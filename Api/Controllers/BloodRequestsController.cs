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
using Api.Dto;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

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
		public async Task<ActionResult<RequestStatusDto>> CheckRequest(int id)
		{
			var request = await _context.BloodRequests.FirstOrDefaultAsync(e => e.Id == id);
			if (request == null)
				return NotFound();
			if (!string.IsNullOrWhiteSpace(request.Status))
				return new ActionResult<RequestStatusDto>(new RequestStatusDto { Status = request.Status, Message = "Already completed" });
			if (request.RequestDateTime.AddDays(request.DurationOfSearch) < DateTime.Now)
			{
				request.Status = "Expired";
				_context.Entry(request).State = EntityState.Modified;
				await _context.SaveChangesAsync();
				return new ActionResult<RequestStatusDto>(new RequestStatusDto { Status = request.Status, Message = "Request expired" });
			}
			var activeDonations = _context.Donations.Where(d => d.BloodType == request.BloodType && !_context.BloodRequestDonation.Any(brd => brd.DonationId == d.Id));

			var donors = new List<Donor>();
			var sum = 0;
			foreach (var d in activeDonations)
			{
				var amount = Math.Min(request.NoOfUnits, d.NoOfUnits);
				sum += amount;
				await _context.BloodRequestDonation.AddAsync(new BloodRequestDonation
				{
					BloodRequestId = request.Id,
					DonationId = d.Id,
					NoOfUnits = amount,
					TransactionDateTime = DateTime.Now
				});
				var donor = _context.Donors.FirstOrDefault(dn => dn.Id == d.DonorId);
				if (donor != null && !string.IsNullOrEmpty(donor.Email))
					donors.Add(donor);

				if (sum >= request.NoOfUnits)
					break;
			}
			if (sum == request.NoOfUnits)
			{
				request.Status = "Done";
				_context.Entry(request).State |= EntityState.Modified;
				await _context.SaveChangesAsync();
				foreach (var donor in donors)
				{
					await _sendgridEmailSender.SendEmailAsync(donor.Email, "Your blood donation", $"Your blood has been donated at {DateTime.Now}");
				}
				await _sendgridEmailSender.SendEmailAsync(request.Email, "Your blood request", $"Your blood has been processed and fulfilled at {DateTime.Now}");
				return new ActionResult<RequestStatusDto>(new RequestStatusDto { Status = request.Status, Message = "Completed" });
			}
			await _sendgridEmailSender.SendEmailAsync(request.Email, "Your blood request", $"Sorry to inform that there is no suitable blood. {DateTime.Now}");
			return new ActionResult<RequestStatusDto>(new RequestStatusDto { Status = request.Status, Message = "Not able to fulfill" });
		}

	}
}
