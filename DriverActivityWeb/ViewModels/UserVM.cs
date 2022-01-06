using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace DriverActivityWeb.ViewModels
{
    public class UserVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        //[Required]
        public string Username { get; set; }

        [JsonIgnore]
        //[Required]
        public string Password { get; set; }
    }
}
