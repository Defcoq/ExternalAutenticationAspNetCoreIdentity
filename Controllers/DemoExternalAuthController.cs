using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
namespace ExampleApp.Controllers
{
    class UserRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
        public string Token { get; set; }
    }
    public class ExternalAuthInfo
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string redirect_uri { get; set; }
        public string scope { get; set; }
        public string state { get; set; }
        public string response_type { get; set; }
        public string grant_type { get; set; }
        public string code { get; set; }
    }
    public class DemoExternalAuthController : Controller
    {
        private static string expectedID = "MyClientID";
        private static string expectedSecret = "MyClientSecret";
        private static List<UserRecord> users = new List<UserRecord> {
            new UserRecord() {
                Id = "1", Name = "Alice", EmailAddress = "alice@example.com",
                Password = "myexternalpassword"
            },
             new UserRecord {
                Id = "2", Name = "Dora", EmailAddress = "dora@example.com",
                Password = "myexternalpassword"
             }
        };
        public IActionResult Authenticate([FromQuery] ExternalAuthInfo info)
         => expectedID == info.client_id ? View((info, string.Empty))
                 : View((info, "Unknown Client"));
    }
}
