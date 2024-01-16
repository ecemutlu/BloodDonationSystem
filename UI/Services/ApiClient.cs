using Api.Models;
using Microsoft.AspNetCore.Mvc;
using UI.Dto;

namespace UI.Services
{
	public class ApiClient
	{
		AuthService _authService;
		string? _apiUrl;

		public string ApiUrl { get => _apiUrl ?? ""; set => _apiUrl = value; }

		public ApiClient(AuthService authService, IConfiguration configuration)
		{
			_authService = authService;
			ApiUrl = configuration["ApiUrl"] ?? "";
		}

		public async Task<IEnumerable<DonorDto>> QueryDonors(LoginDto loginDto, string name)
		{
			HttpClient httpClient = await CreateClientAsync(loginDto);
			var donors = await httpClient.GetFromJsonAsync<IEnumerable<DonorDto>>($"/api/v1/donors/byname?name={name}");
			return donors ?? new List<DonorDto>();
		}

		public async Task<DonorDto?> GetDonor(LoginDto loginDto, int id)
		{
			HttpClient httpClient = await CreateClientAsync(loginDto);
			return await httpClient.GetFromJsonAsync<DonorDto>($"/api/v1/donors/{id}");
		}

		public async Task UpdateDonor(LoginDto loginDto, int id, DonorDto donor)
		{
			var httpClient = await CreateClientAsync(loginDto);
			var responseMessage = await httpClient.PutAsJsonAsync($"/api/v1/donors/{id}", donor);
			responseMessage.EnsureSuccessStatusCode();
			await responseMessage.Content.ReadFromJsonAsync<DonorDto>();
		}

		private async Task<HttpClient> CreateClientAsync(LoginDto? loginDto)
		{
			var httpClient = new HttpClient
			{
				BaseAddress = new Uri(ApiUrl ?? "")
			};
			if (loginDto != null)
			{
				var token = await _authService.GetTokenAsync(loginDto);
				httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
			}
			return httpClient;
		}

		public async Task<IEnumerable<DonorDto>> QueryDonors(LoginDto loginDto)
		{
			var httpClient = await CreateClientAsync(loginDto);            
            var donors = await httpClient.GetFromJsonAsync<IEnumerable<DonorDto>>($"/api/v1/donors");
            return donors ?? new List<DonorDto>();
		}

		public async Task<DonorDto?> CreateDonor(LoginDto loginDto, DonorDto donorDto)
		{
			var httpClient = await CreateClientAsync(loginDto);
			var responseMessage = await httpClient.PostAsJsonAsync($"/api/v1/donors", donorDto);
			responseMessage.EnsureSuccessStatusCode();
			return await responseMessage.Content.ReadFromJsonAsync<DonorDto>();
		}

		public async Task<DonationDto?> CreateDonation(LoginDto loginDto, DonationDto donationDto)
		{
			var httpClient = await CreateClientAsync(loginDto);
			var responseMessage = await httpClient.PostAsJsonAsync($"/api/v1/donations", donationDto);
			responseMessage.EnsureSuccessStatusCode();
			return await responseMessage.Content.ReadFromJsonAsync<DonationDto>();
		}

		public async Task<IEnumerable<CityDto>> QueryCities()
		{
			var httpClient = await CreateClientAsync(null);
			var cities = await httpClient.GetFromJsonAsync<IEnumerable<CityDto>>($"/api/v1/Cities");
			return cities ?? new List<CityDto>();
		}

		public async Task<IEnumerable<TownDto>> QueryTowns()
		{
			var httpClient = await CreateClientAsync(null);
			var towns = await httpClient.GetFromJsonAsync<IEnumerable<TownDto>>($"/api/v1/Towns");
			return towns ?? new List<TownDto>();
		}

		public async Task<BloodRequestDto?> CreateRequest(LoginDto loginDto, BloodRequestDto bloodRequestDto)
		{
			var httpClient = await CreateClientAsync(loginDto);
			var responseMessage = await httpClient.PostAsJsonAsync($"/api/v1/bloodrequests", bloodRequestDto);
			responseMessage.EnsureSuccessStatusCode();
			return await responseMessage.Content.ReadFromJsonAsync<BloodRequestDto>();
		}
		public async Task<string?> UploadPhoto(LoginDto loginDto, PhotoDto photoDto)
		{
			var httpClient = await CreateClientAsync(loginDto);
			var responseMessage = await httpClient.PostAsJsonAsync($"/api/v1/donors/postphoto", photoDto);
			responseMessage.EnsureSuccessStatusCode();
			return await responseMessage.Content.ReadAsStringAsync();
		}
	}
}
