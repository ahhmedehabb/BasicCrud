using BasicCrudOperation.Enums;

namespace BasicCrudOperation.Models
{
	public class UserPermissions
	{
		public int UserId { get; set; }

		public Permission PermissionId { get; set; }
	}
}
