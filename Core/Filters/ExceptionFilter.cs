using Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Core.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly IHostEnvironment _hostEnvironment;

    public ExceptionFilter(IHostEnvironment hostEnvironment) =>
        _hostEnvironment = hostEnvironment;

    public void OnException(ExceptionContext context)
    {
        var response = context.HttpContext.Response;
        response.ContentType = "application/json";

        if (context.Exception is BusinessException)
        {
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Result = new JsonResult(new { error = context.Exception.Message });
            return;
        }

        // response.StatusCode = (int)HttpStatusCode.InternalServerError;
        // context.Result =  new JsonResult(new { error = "Server Error" });
    }
}