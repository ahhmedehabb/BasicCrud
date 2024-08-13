using System.Diagnostics;

namespace AspNetCore.MiddleWare
{
	public class ProfilingMiddleWare
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ProfilingMiddleWare> _logger;

		public ProfilingMiddleWare(RequestDelegate next,ILogger<ProfilingMiddleWare> logger)
		{
			_next=next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			var stopWatch = new Stopwatch();
			stopWatch.Start();
			await _next(context);
			stopWatch.Stop();
			_logger.LogInformation($"Request '{context.Request.Path}' took '{stopWatch.ElapsedMilliseconds}ms' to excued");
		}
	}
}
