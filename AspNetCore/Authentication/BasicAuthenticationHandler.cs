using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace AspNetCore.Authentication
{
	public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
	{
		public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options
			, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
		{
		}

		protected override Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			if (!Request.Headers.ContainsKey("Authorization"))
				return Task.FromResult(AuthenticateResult.NoResult());

			if (!AuthenticationHeaderValue.TryParse(Request.Headers["Authorization"],out var authHeader))
				return Task.FromResult(AuthenticateResult.Fail("Unknown Schema"));

			if(!authHeader.Scheme.Equals("Basic",StringComparison.OrdinalIgnoreCase))
				return Task.FromResult(AuthenticateResult.Fail("Unknown Schema"));


			var encodedCredentials = authHeader.Parameter;
			var decodedCredentials=Encoding.UTF8.GetString( Convert.FromBase64String(encodedCredentials));
			var usernameAndPassword = decodedCredentials.Split(':');
			if (usernameAndPassword[0] != "admin" || usernameAndPassword[1] != "password")
				return Task.FromResult(AuthenticateResult.Fail("Invalid Username and password"));

			var identity = new ClaimsIdentity(new Claim[] { 
				new Claim(ClaimTypes.NameIdentifier,"1"),
				new Claim(ClaimTypes.Name,usernameAndPassword[0])
			}, "Basic");

			var prinicipal=new ClaimsPrincipal(identity);
			var Ticket = new AuthenticationTicket(prinicipal, "Basic");
			return Task.FromResult(AuthenticateResult.Success(Ticket));
		}
	}
}
