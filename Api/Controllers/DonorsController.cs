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
using Api.Dto;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;

namespace Api.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class DonorsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private IConfiguration _config;

		public DonorsController(ApplicationDbContext context, IConfiguration configuration)
		{
			_context = context;
			_config = configuration;
		}

		// GET: api/Donors
		[HttpGet]
		[Authorize]
		public async Task<ActionResult<IEnumerable<Donor>>> GetDonors()
		{
			return await _context.Donors.ToListAsync();
		}

		// GET: api/Donors/byname
		[HttpGet("byname")]
		[Authorize]
		public async Task<ActionResult<IEnumerable<Donor>>> GetDonorByName(string name)
		{
			return await _context.Donors
							.Where(d => d.Name.StartsWith(name))
							.ToListAsync();
		}

		// GET: api/Donors/5

		[HttpGet("{id}")]
		[Authorize]
		public async Task<ActionResult<Donor>> GetDonorById(int id)
		{
			var donor = await _context.Donors.FindAsync(id);

			if (donor == null)
			{
				return NotFound();
			}

			return donor;
		}

		// PUT: api/Donors/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		[Authorize]
		public async Task<IActionResult> PutDonor(int id, Donor donor)
		{
			if (id != donor.Id)
			{
				return BadRequest();
			}

			_context.Entry(donor).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!DonorExists(id))
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

		// POST: api/Donors
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		[Authorize]
		public async Task<ActionResult<Donor>> PostDonor(Donor donor)
		{
			_context.Donors.Add(donor);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetDonorById", new { id = donor.Id }, donor);
		}

		// DELETE: api/Donors/5
		[HttpDelete("{id}")]
		[Authorize]
		public async Task<IActionResult> DeleteDonor(int id)
		{
			var donor = await _context.Donors.FindAsync(id);
			if (donor == null)
			{
				return NotFound();
			}

			_context.Donors.Remove(donor);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool DonorExists(int id)
		{
			return _context.Donors.Any(e => e.Id == id);
		}

		[HttpPost("postphoto")]
		[Authorize]
		public async Task<ActionResult<string>> PostDonorPhoto(PhotoDto photoDto)
		{
			var donor = await _context.Donors.FindAsync(photoDto.DonorId);
			if (donor == null)
			{
				return NotFound();
			}

			donor.Image = await UploadPhoto(photoDto);
			_context.Entry(donor).State = EntityState.Modified;

			await _context.SaveChangesAsync();

			return Ok(donor.Image);
		}

		private async Task<string> UploadPhoto(PhotoDto photoDto)
		{
			var fileName = Guid.NewGuid().ToString().Replace("-", "") + photoDto.FileExtension;
			var connectionString = _config["BlobStorage:ConnectionString"];
			var containerClient = new BlobContainerClient(connectionString, "bloodbank");
			var blobClient = containerClient.GetBlobClient(fileName);
			await blobClient.UploadAsync(new BinaryData(photoDto.Content), new BlobUploadOptions
			{
				HttpHeaders = new BlobHttpHeaders
				{
					ContentType = GetMimeType(fileName)
				}
			});
			return fileName;
		}

		private static string GetMimeType(string fileName)
		{
			const string defaultType = "application/octet-stream";
			var ext = Path.GetExtension(fileName);
			return ext.ToLowerInvariant() switch
			{
				".jpg" or ".jpeg" => "image/jpeg",
				".png" => "image/png",
				_ => defaultType,
			};
		}

	}
}
