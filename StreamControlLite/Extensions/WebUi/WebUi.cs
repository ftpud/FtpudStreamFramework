using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using StreamControlLite.Extensions.Model;
using StreamControlLite.Extensions.WebUi.Auth;

namespace StreamControlLite.Extensions.WebUi
{

    public class PlayListDto
    {
        public List<PlaylistItem> Playlist { get; set; }
    }

    public class WebUi
    {
        private WebServer _webServer;

        public void Init(int port)
        {
            _webServer = new WebServer($"http://*:{port}/");
            
            _webServer.AddPage("/list", new AuthIntercepterObject(new SimpleAuth("pass", "main", "1"), request =>
            {
                var playList = MediaLibrary.instance().GetPlaylist();
                return new WebServerResponse(JsonConvert.SerializeObject(new PlayListDto() {Playlist =playList}));
            }));
            
            _webServer.AddPage("/", new AuthIntercepterObject(new SimpleAuth("pass", "main", "1"), request =>
            {
                return new WebServerResponse(File.ReadAllText("./Html/index.html"));
            }));

            
            
            _webServer.AddPage("/current", new AuthIntercepterObject(new SimpleAuth("pass", "main", "1"), request =>
            {
                return new WebServerResponse(MediaLibrary.instance().GetCurrentSongNumber().ToString());
            }));
            
            _webServer.AddPage("/set", new AuthIntercepterObject(new SimpleAuth("pass", "main", "1"), request =>
            {
                int itemNumber = 0;
                int.TryParse(request.QueryString.Get("number"), out itemNumber);
                MediaLibrary.instance().PlayItemWithNumber(itemNumber);
                
                return new WebServerResponse("ok");
            }));
            
            _webServer.Run();
        }

        
        


        private static WebUi _instance;

        public static WebUi instance()
        {
            if (_instance == null)
            {
                _instance = new WebUi();
            }

            return _instance;
        }
    }
}