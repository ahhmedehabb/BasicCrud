using AspNetCore.Data;
using AspNetCore.Filters;
using BasicCrudOperation.Authorizations;
using BasicCrudOperation.Enums;
using BasicCrudOperation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Security.Claims;

namespace AspNetCore.Controllers
{
	[ApiController]
	[Route("[Controller]")]
	[Authorize]

	public class ProductsController : ControllerBase
	{
		private readonly ApplicationDBContext _dbContext;
		private readonly ILogger<ProductsController> _logger;

		public ProductsController(ApplicationDBContext applicationDBContext,ILogger<ProductsController> logger)
		{
			_dbContext = applicationDBContext;
			_logger = logger;
		}

		[HttpPost]
		[Route("/Create")]
		public ActionResult<int> CreateProduct(Product product)
		{
			product.Id = 0;
			_dbContext.Set<Product>().Add(product);
			_dbContext.SaveChanges();
			return Ok(product.Id);
		}
		[HttpGet]
		[Route("/GetAllProducts")]
		//[CheckPermission(Permission.ReadProducts)]  - Permission-Based
		//[Authorize(Roles = "Admin")] - Role-Based
		[Authorize(Policy = "AgeGreaterThan25")] //- Policy-Based
		public ActionResult<IEnumerable<Product>> GetAllProducts()
		{
			var isAdmin = User.IsInRole("Admin");
			var userName = User.Identity.Name;
			var userID=((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var records = _dbContext.Set<Product>().ToList();
			return records==null ? NotFound("no records found") : Ok(records);
		}

		[HttpGet]
		[Route("{id}")]
		//[CheckPermission(Permission.ReadProducts)]
		[LogSensitiveAction]
		public ActionResult<Product> GetProductById(int id)
		{
			_logger.LogDebug($"Getting Product #"+id);
			var product = _dbContext.Set<Product>().Find(id);
			if (product == null)
				_logger.LogWarning("Product #{x} was not found-- time {y}", id,DateTime.Now);
			return product == null ? NotFound("no records found") : Ok(product);
		}

		[HttpPut]
		[Route("/updateProducts")]
		public ActionResult updateProduct(Product product)
		{
			var existingProduct = _dbContext.Set<Product>().Find(product.Id);
			if (existingProduct != null)
			{
				existingProduct.Name = product.Name;
				existingProduct.Sku = product.Sku;
				_dbContext.Set<Product>().Update(existingProduct);
				_dbContext.SaveChanges();
				return Ok();
			}
			else
				return NotFound();
		}

		[HttpDelete]
		[Route("{id}")]

		public ActionResult DeleteProduct(int id)
		{
			var existingProduct = _dbContext.Set<Product>().Find(id);
			if (existingProduct != null)
			{
				_dbContext.Set<Product>().Remove(existingProduct);
				_dbContext.SaveChanges();
				return Ok();
			}
			else return NotFound();
		}


	}
}
