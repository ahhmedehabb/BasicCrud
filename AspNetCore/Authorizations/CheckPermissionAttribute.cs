using BasicCrudOperation.Enums;
using Microsoft.Identity.Client.Extensibility;

namespace BasicCrudOperation.Authorizations
{
	[AttributeUsage(AttributeTargets.Method,AllowMultiple =false)]
	public class CheckPermissionAttribute:Attribute
	{
	

		public CheckPermissionAttribute(Permission permission)
		{
			Permission = permission;
		}

		public Permission Permission { get; }
	}
}
