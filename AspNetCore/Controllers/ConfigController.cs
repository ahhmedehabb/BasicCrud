using BasicCrudOperation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AspNetCore.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class ConfigController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly IOptionsMonitor<AttachmentOptions> _attachmentOptions;

		public ConfigController(IConfiguration configuration, IOptionsMonitor<AttachmentOptions> attachmentOptions)
		{
			_configuration = configuration;
			_attachmentOptions = attachmentOptions;
			var value=attachmentOptions.CurrentValue;
		}
		[HttpGet]
		[Route("")]
		public ActionResult GetConfig()
		{
			Thread.Sleep(10000);
			var config = new {
				EnvName = _configuration["ASPNETCORE_ENVIRONMENT"],
				AllowedHosts = _configuration["AllowedHosts"],
				DefaultConnection = _configuration.GetConnectionString("DefaultConnection"),
				DefaultLogLevel = _configuration["Logging:LogLevel:Default"],
				TestKey = _configuration["TestKey"],
				AttactmentsOptions=_attachmentOptions.CurrentValue,
			};
			return Ok (config);
		}
	}
}
