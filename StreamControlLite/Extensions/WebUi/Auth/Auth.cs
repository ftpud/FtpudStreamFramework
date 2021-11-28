using System;
using System.Net;

namespace StreamControlLite.Extensions.WebUi.Auth
{
    public class AuthIntercepterObject : ResponseExtension
    {
        public Func<HttpListenerRequest, WebServerResponse> originalReply { get; set; }
        public Func<HttpListenerRequest, WebServerResponse> interceptor { get; set; }
        public SimpleAuth auth { get; set; }

        public AuthIntercepterObject(SimpleAuth auth, Func<HttpListenerRequest, WebServerResponse> originalReply)
        {
            this.originalReply = originalReply;
            this.auth = auth;
        }


        public override WebServerResponse intercept(HttpListenerRequest request)
        {
            if (auth.isAuthorized(request))
            {
                return originalReply.Invoke(request);
            }
            else
            {
                return auth.intercept(request);
            }
        }
    }

}