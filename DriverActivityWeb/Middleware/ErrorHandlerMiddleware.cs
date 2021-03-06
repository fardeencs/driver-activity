using DriverActivityWeb.ViewModels;
using FluentValidation;
using System.Net;
using System.Text.Json;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var responseModel = ApiResponse<string>.Fail(error.Message);
            var exceptionType = error.GetType();
            //if (exceptionType == typeof(ValidationException))
            //{
            //    var resp = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(error.Message), ReasonPhrase = "ValidationException", };
            //    response.StatusCode = (int)HttpStatusCode.BadRequest;
            //    //r result = JsonSerializer.Serialize(resp);
            //}
            switch (error)
            {
                case CustomException e:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException e:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case UnauthorizedAccessException e:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.Redirect("/login/home");
                    break;
                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            var result = JsonSerializer.Serialize(responseModel);
            await response.WriteAsync(result);
        }
    }
}