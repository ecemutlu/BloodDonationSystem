using System;
using System.Diagnostics;
using System.Drawing;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UI.Dto;
using UI.Models;
using UI.Services;

namespace UI.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ApiClient _apiClient;
		private readonly BranchDto _branch;
		private readonly LoginDto _loginDto;
		private readonly IConfiguration _configuration;
		public HomeController(ApiClient apiClient, ILogger<HomeController> logger, MyBranch myBranch, IConfiguration configuration)
		{
			_apiClient = apiClient;
			_logger = logger;
			_branch = myBranch.Branch;
			_loginDto = myBranch.GetLoginInfo();
			_configuration = configuration;
		}
		[HttpGet]
		public async Task<IActionResult> CreateDonor()
		{
			ViewData["Branch"] = _branch;
			IEnumerable<CityDto> cities = await _apiClient.QueryCities();
			ViewData["City"] = cities;
			IEnumerable<TownDto> towns = await _apiClient.QueryTowns();
			ViewData["Town"] = towns;

			return View();
		}
		[HttpPost]
		public async Task<IActionResult> CreateDonor([Bind("Name,CityId,TownId,PhoneNo,Email,BloodType,Photo")] DonorDto donor)
		{
			ViewData["Branch"] = _branch;
			ViewData["imgPrefix"] = _configuration["CDNPrefix"];
			ViewData["Branch"] = _branch;
			IEnumerable<CityDto> cities = await _apiClient.QueryCities();
			ViewData["City"] = cities;
			IEnumerable<TownDto> towns = await _apiClient.QueryTowns();
			ViewData["Town"] = towns;
			try
			{
				if (ModelState.IsValid)
				{
					var newDonor = await _apiClient.CreateDonor(_loginDto, donor);

					if (donor.Photo == null)
						return View(newDonor);


					var photoDto = new PhotoDto
					{
						DonorId = newDonor!.Id,
						FileExtension = Path.GetExtension(donor.Photo.FileName),
						Content = []
					};
					using (var ms = new MemoryStream())
					{
						donor.Photo.CopyTo(ms);
						photoDto.Content = ms.ToArray();
					}
					newDonor.Image = await _apiClient.UploadPhoto(_loginDto, photoDto);
					return View(newDonor);
				}
			}
			catch (Exception /* ex */)
			{
				ModelState.AddModelError("", "Unable to save changes. " +
					"Try again, and if the problem persists " +
					"see your system administrator.");
			}

			return View(donor);
		}

		[HttpGet]
		public async Task<IActionResult> EditDonor(int id)
		{
			ViewData["Branch"] = _branch;
			ViewData["imgPrefix"] = _configuration["CDNPrefix"];
			ViewData["Branch"] = _branch;
			IEnumerable<CityDto> cities = await _apiClient.QueryCities();
			ViewData["City"] = cities;
			IEnumerable<TownDto> towns = await _apiClient.QueryTowns();
			ViewData["Town"] = towns;
			var donor = await _apiClient.GetDonor(_loginDto, id);
			return View(donor);
		}
		[HttpPost]
		public async Task<IActionResult> EditDonor([Bind("Name,CityId,TownId,PhoneNo,Email,BloodType,Photo")] DonorDto donor)
		{
			ViewData["Branch"] = _branch;
			ViewData["imgPrefix"] = _configuration["CDNPrefix"];
			ViewData["Branch"] = _branch;
			IEnumerable<CityDto> cities = await _apiClient.QueryCities();
			ViewData["City"] = cities;
			IEnumerable<TownDto> towns = await _apiClient.QueryTowns();
			ViewData["Town"] = towns;
			try
			{
				if (ModelState.IsValid)
				{
					await _apiClient.UpdateDonor(_loginDto,donor.Id, donor);

					if (donor.Photo == null)
						return View(donor);


					var photoDto = new PhotoDto
					{
						DonorId = donor!.Id,
						FileExtension = Path.GetExtension(donor.Photo.FileName),
						Content = []
					};
					using (var ms = new MemoryStream())
					{
						donor.Photo.CopyTo(ms);
						photoDto.Content = ms.ToArray();
					}
					donor.Image = await _apiClient.UploadPhoto(_loginDto, photoDto);
					return RedirectToAction("ListDonors");
				}
			}
			catch (Exception /* ex */)
			{
				ModelState.AddModelError("", "Unable to save changes. " +
					"Try again, and if the problem persists " +
					"see your system administrator.");
			}

			return View(donor);
		}


		[HttpGet]
		public async Task<IActionResult> ListDonors()
		{
			var donors = await _apiClient.QueryDonors(_loginDto);
			ViewData["Donor"] = donors;
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> RequestBlood()
		{
			ViewData["Branch"] = _branch;
			IEnumerable<CityDto> cities = await _apiClient.QueryCities();
			ViewData["City"] = cities;
			IEnumerable<TownDto> towns = await _apiClient.QueryTowns();
			ViewData["Town"] = towns;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> RequestBlood([Bind("Requester,Reason,DurationOfSearch,CityId,TownId,NoOfUnits,Email,BloodType,RequestDateTime")] BloodRequestDto bloodRequest)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var newRequest = await _apiClient.CreateRequest(_loginDto, bloodRequest);
					TempData["testmsg"] = "Request successfully sent!";
				}
				ViewData["Branch"] = _branch;
				IEnumerable<CityDto> cities = await _apiClient.QueryCities();
				ViewData["City"] = cities;
				IEnumerable<TownDto> towns = await _apiClient.QueryTowns();
				ViewData["Town"] = towns;
				return View();
			}
			catch (DbUpdateException /* ex */)
			{
				ModelState.AddModelError("", "Unable to save changes. " +
					"Try again, and if the problem persists " +
					"see your system administrator.");
			}
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> AddBlood()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> AddBlood([Bind("DonorId,BranchId,NoOfUnits,BloodType")] DonationDto donation)
		{
			ViewData["Branch"] = _branch;
			try
			{
				if (ModelState.IsValid)
				{
					var updatedUnits = await _apiClient.CreateDonation(_loginDto, donation);
					return RedirectToAction(nameof(ListDonors));
				}
			}
			catch (DbUpdateException /* ex */)
			{
				ModelState.AddModelError("", "Unable to save changes. " +
					"Try again, and if the problem persists " +
					"see your system administrator.");
			}
			return View(donation);
		}
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
