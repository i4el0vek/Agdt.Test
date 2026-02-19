using Agdt.Host.Models;
using System.Net;

namespace Agdt.Host.Middleware;

public class ExceptionMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ExceptionMiddleware> _logger;

	public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Catch exception in middleware");
			await HandleExceptionAsync(context, ex);
		}
	}

	private static Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		var code = HttpStatusCode.InternalServerError;
		var message = "Internal server error";

		if (exception is KeyNotFoundException)
		{
			code = HttpStatusCode.NotFound;
			message = exception.Message;
		}

		var result = new ErrorModel((int)code, message);
		
		context.Response.StatusCode = result.Code;
		return context.Response.WriteAsJsonAsync(result);
	}
}