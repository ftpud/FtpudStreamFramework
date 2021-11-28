using System.Net;

namespace StreamControlLite.Extensions.WebUi.Auth
{
    public abstract class ResponseExtension
    {
        public abstract WebServerResponse intercept(HttpListenerRequest request);
    }
}