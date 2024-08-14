using AspNetCore.Data;
using BasicCrudOperation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace BasicCrudOperation.Authorizations
{
	public class PermissionBasedAuthorizationFilter(ApplicationDBContext dBContext) : IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			var attribute = (CheckPermissionAttribute)context.ActionDescriptor.EndpointMetadata
				.FirstOrDefault(x => x is CheckPermissionAttribute);
			if (attribute != null)
			{
				var claimIdetity = context.HttpContext.User.Identity as ClaimsIdentity;
				if (claimIdetity == null || !claimIdetity.IsAuthenticated)
					context.Result = new ForbidResult();
				else
				{
					var userId = int.Parse(claimIdetity.FindFirst(ClaimTypes.NameIdentifier).Value);
					bool hasPermissions = dBContext.Set<UserPermissions>()
					.Any(x => x.UserId == userId && x.PermissionId == attribute.Permission);
					if(!hasPermissions)
						context.Result = new ForbidResult();
				}
			}
		}
	}
}
