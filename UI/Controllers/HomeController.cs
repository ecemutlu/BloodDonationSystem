using System.Diagnostics;
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
		public HomeController(ApiClient apiClient, ILogger<HomeController> logger, MyBranch myBranch)
		{
			_apiClient = apiClient;
			_logger = logger;
			_branch = myBranch.Branch;
			_loginDto = myBranch.GetLoginInfo();
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
		public async Task<IActionResult> CreateDonor([Bind("Name,Image,CityId,TownId,PhoneNo,Email,BloodType")] DonorDto donor)
		{
			ViewData["Branch"] = _branch;
			try
			{
				if (ModelState.IsValid)
				{
					var newDonor = await _apiClient.CreateDonor(_loginDto,donor);
					
					return RedirectToAction(nameof(ListDonors));
				}
			}
			catch (DbUpdateException /* ex */)
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

		public async Task<IActionResult> RequestBlood()
		{
			return View();
		}
		public async Task<IActionResult> AddBlood()
		{
			return View();
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
