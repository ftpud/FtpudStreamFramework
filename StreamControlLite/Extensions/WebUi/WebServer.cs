using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using StreamControlLite.Extensions.WebUi.Auth;

namespace StreamControlLite.Extensions.WebUi
{
  public class WebServer
    {
        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest, WebServerResponse> _responderMethod;

        private readonly Dictionary<string, Func<HttpListenerRequest, WebServerResponse>> pageList =
            new Dictionary<string, Func<HttpListenerRequest, WebServerResponse>>();

        public WebServer(params string[] prefixes)
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException(
                    "Needs Windows XP SP2, Server 2003 or later.");

            // URI prefixes are required, for example 
            // "http://localhost:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // A responder method is required
            //if (method == null)
            //    throw new ArgumentException("method");

            foreach (var s in prefixes)
                _listener.Prefixes.Add(s);

            _responderMethod = responder;
            _listener.Start();
        }

        public void AddPage(string exp, Func<HttpListenerRequest, WebServerResponse> reply)
        {
            pageList.Add(exp, reply);
        }

        public void AddPage(string exp, ResponseExtension responseExtension)
        {
            pageList.Add(exp, responseExtension.intercept);
        }

        private WebServerResponse responder(HttpListenerRequest request)
        {
            return pageList.Where(x => x.Key == request.Url.AbsolutePath).FirstOrDefault().Value.Invoke(request);
        }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                Console.WriteLine("Webserver running...");
                try
                {
                    while (_listener.IsListening)
                        ThreadPool.QueueUserWorkItem(c =>
                        {
                            var ctx = c as HttpListenerContext;
                            ctx.Response.AppendHeader("Access-Control-Allow-Origin", "*");
                            ctx.Response.AppendHeader("Access-Control-Allow-Methods", "GET");
                            ctx.Response.ContentEncoding = Encoding.UTF8;
                            //ctx.Response.ContentType = "text/plain; charset=utf-8";
                            try
                            {
                                var rstr = _responderMethod(ctx.Request);
                                var buf = Encoding.UTF8.GetBytes(rstr.payload);
                                ctx.Response.Cookies.Add(rstr.cookies);
                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                            }
                            catch
                            {
                            } // suppress any exceptions
                            finally
                            {
                                // always close the stream
                                ctx.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                }
                catch
                {
                } // suppress any exceptions
            });
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }
    }
}