using System;
using System.IO;
using System.Linq;
using System.Net;

namespace StreamControlLite.Extensions.WebUi.Auth
{
    public class SimpleAuth
    {
        private string passw;
        private string key;
        private string value;

        public SimpleAuth(string password, string key, string value)
        {
            this.key = key;
            this.passw = password;
            this.value = value;
        }

        public WebServerResponse intercept(HttpListenerRequest request)
        {
            using (StreamReader sr = new StreamReader(request.InputStream))
            {
                String s = sr.ReadToEnd();
                if (s.Equals($"pass={passw}"))
                {
                    var cookie = new Cookie(key, value);
                    var cookieCollection = new CookieCollection();
                    cookieCollection.Add(cookie);
                    return new WebServerResponse()
                    {
                        payload = File.ReadAllText("./Html/welcome.html"),
                        cookies = cookieCollection
                    };
                }
            }

            return new WebServerResponse(File.ReadAllText("./Html/login.html"));
        }

        public bool isAuthorized(HttpListenerRequest request)
        {
            return request.Cookies.Any(c => c.Name.Equals(key) && c.Value.Equals(value));
        }
    }
}