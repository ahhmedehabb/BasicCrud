using BasicCrudOperation.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Data
{
	public class ApplicationDBContext : DbContext
	{
		public ApplicationDBContext(DbContextOptions options) : base(options) { }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Product>().ToTable("Products");
			modelBuilder.Entity<Users>().ToTable("Users");
			modelBuilder.Entity<UserPermissions>().ToTable("UserPermissions")
				.HasKey(x=>new { x.UserId,x.PermissionId});
		}
	}
}
