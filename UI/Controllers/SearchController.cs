using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UI.Services;

namespace UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ApiClient _apiClient;

        public SearchController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }
    }
}
