using System.Net;

namespace DriverActivityWeb.ViewModels
{
    public class ResponseEntity
    {
        public object? Data { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public ResponseEntity(object data)
        {
            this.Data = data;
            this.HttpStatusCode = HttpStatusCode.OK;
        }

        public ResponseEntity(object data, List<string> errors)
        {
            this.Data = data;
            this.HttpStatusCode = HttpStatusCode.BadRequest;
            this.Errors = errors;
        }

        public ResponseEntity(List<string> errors)
        {
            this.HttpStatusCode = HttpStatusCode.BadRequest;
            this.Errors = errors;
        }
    }

    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public static ApiResponse<T> Fail(string errorMessage)
        {
            return new ApiResponse<T> { Succeeded = false, Message = errorMessage };
        }
        public static ApiResponse<T> Success(T data)
        {
            return new ApiResponse<T> { Succeeded = true, Data = data };
        }
    }
}
