using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace AspNetCore.Filters
{
	public class logActivityFilter : IActionFilter,IAsyncActionFilter
	{
		private readonly ILogger<logActivityFilter> logger;

		public logActivityFilter(ILogger<logActivityFilter> logger)
		{
			this.logger = logger;
		}


		public void OnActionExecuting(ActionExecutingContext context)
		{
			logger.LogInformation($"Execution Action {context.ActionDescriptor.DisplayName} on controller {context.Controller} with arguments {JsonSerializer.Serialize( context.ActionArguments)}");
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
			logger.LogInformation($"Action {context.ActionDescriptor.DisplayName} Finised on Controller {context.Controller}");
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			logger.LogInformation($"(Async) Execution Action {context.ActionDescriptor.DisplayName} on controller {context.Controller} with arguments {JsonSerializer.Serialize(context.ActionArguments)}");

			await next();

			logger.LogInformation($"(Async) Action {context.ActionDescriptor.DisplayName} Finised on Controller {context.Controller}");
		}
	}

}
