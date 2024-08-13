using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Data
{
	public class Product
	{ 
		public required int Id { get; set; }

	
		public required string Name { get; set; }

	
		public required string Sku { get; set; }
	}
}
