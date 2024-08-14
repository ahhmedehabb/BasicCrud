using AspNetCore.Data;
using BasicCrudOperation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BasicCrudOperation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController(JwtOptions jwtOptions,ApplicationDBContext dBContext) : ControllerBase
	{
		[HttpPost]
		[Route("auth")]
		public ActionResult<string> AuthenticateUser(AuthenticationRequest request)
		{
			var user = dBContext.Set<Users>().FirstOrDefault(x => x.Name == request.UserName && x.Password == request.Password);
			if (user == null)
				 return Unauthorized(); 
			var TokenHandler=new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Issuer = jwtOptions.Issuer,
				Audience = jwtOptions.Audience,
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
				, SecurityAlgorithms.HmacSha256),
				Subject = new ClaimsIdentity(new Claim[]
				{
					new(ClaimTypes.NameIdentifier,user.ID.ToString()),
					new(ClaimTypes.NameIdentifier,user.Name)
				})
			};
			var securityToken = TokenHandler.CreateToken(tokenDescriptor);
			var accessToken=TokenHandler.WriteToken(securityToken);
			return Ok(accessToken);
		}
	}
}
