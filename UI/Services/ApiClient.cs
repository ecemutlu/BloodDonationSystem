using System.Net.Http;
using Api.Models;
using UI.Dto;

namespace UI.Services
{
    public class ApiClient
    {
        AuthService _authService;
        string _apiUrl;

        public ApiClient(AuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _apiUrl = configuration["ApiUrl"] ?? "";
        }

        public async Task<IEnumerable<DonorDto>> QueryDonors(LoginDto loginDto, string name)
        {
            var token = await _authService.GetTokenAsync(loginDto);

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_apiUrl ?? "")
            };
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var donors = await httpClient.GetFromJsonAsync<IEnumerable<DonorDto>>($"/api/v1/donors?name={name}");
            return donors ?? new List<DonorDto>();
        }


    }
}
