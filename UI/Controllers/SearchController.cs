using System.Collections;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using UI.Services;
using UI.Dto;

namespace UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ApiClient _apiClient;
        private readonly MyBranch _myBranch;

        public SearchController(ApiClient apiClient, MyBranch myBranch)
        {
            _apiClient = apiClient;
            _myBranch = myBranch;
        }

        [HttpGet("page")]
        public async Task<IEnumerable<DonorDto>> GetDonors([FromQuery]PageRequest pageRequest)
        {
            return await _apiClient.QueryDonors(_myBranch.GetLoginInfo());
        }

        [HttpGet("DonorsByName")]
        public async  Task<IEnumerable<DonorDto>> GetDonorsByName(string q)
        {
           return await _apiClient.QueryDonors(_myBranch.GetLoginInfo(), q);
        }        
        
        [HttpGet("Cities")]
        public async Task<IEnumerable<CityDto>> GetCities()
        {
            return await _apiClient.QueryCities();
        }
        
        [HttpGet("Towns")]
        public async Task<IEnumerable<TownDto>> GetTowns()
        {
            return await _apiClient.QueryTowns();
        }
    }
}
