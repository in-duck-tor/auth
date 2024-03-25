using InDuckTor.Auth.Models;
using InDuckTor.Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace InDuckTor.Auth.Controllers
{
    [ApiController]
    [Route("credentials")]
    public class CredentialsController : ControllerBase
    {
        private readonly ICredentialsService _credentialsService;

        public CredentialsController(ICredentialsService credentialsService)
        {
            _credentialsService = credentialsService;
        }

        [HttpPost]
        public async Task<ActionResult<long>> Create(CredentialsDto model)
        {
            var id = await _credentialsService.CreateCredentials(model);
            return Ok(id);
        }
    }
}
