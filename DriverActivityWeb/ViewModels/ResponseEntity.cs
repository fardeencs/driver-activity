using Newtonsoft.Json;
using System.Net;

namespace DriverActivityWeb.ViewModels
{
    public class ResponseEntity
    {
        [JsonProperty("data")]
        public object? Data { get; set; }
        [JsonProperty("httpStatusCode")]
        public HttpStatusCode HttpStatusCode { get; set; }
        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }
        [JsonProperty("errors")]
        public List<string> Errors { get; set; } = new List<string>();

        public ResponseEntity(object data)
        {
            this.Data = data;
            this.HttpStatusCode = HttpStatusCode.OK;
            this.Succeeded = true;
        }

        public ResponseEntity(object data, List<string> errors)
        {
            this.Data = data;
            this.HttpStatusCode = HttpStatusCode.BadRequest;
            this.Errors = errors;
            this.Succeeded = false;
        }

        public ResponseEntity(List<string> errors)
        {
            this.HttpStatusCode = HttpStatusCode.BadRequest;
            this.Errors = errors;
            this.Succeeded = false;
        }
    }

    public class ApiResponse<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; }
        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }
        [JsonProperty("message")]
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
