using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FtpudStreamFramewok.Core;
using FtpudStreamFramewok.Source;
using StreamControlLite.Extensions.Model;

namespace StreamControlLite.Extensions
{
    public class MediaLibrary
    {
        private List<PlaylistItem> _playList;

        private bool IsValidVideoExtension(string filename)
        {
            return filename.EndsWith(".ts")
                   || filename.EndsWith(".mp4")
                   || filename.EndsWith(".mkv")
                   || filename.EndsWith(".m2ts");
        }


        private int _currentSongNumber = 0;

        public void Init()
        {

            InputProcessor.Instance().OnFinished += (sender, eventArgs) =>
            {
                _currentSongNumber++;
                if (_currentSongNumber == _playList.Count) _currentSongNumber = 0;
                InputProcessor.Instance().Play(_playList[_currentSongNumber].GetSourceEntity());
            };
        }

        public int GetCurrentSongNumber()
        {
            return _currentSongNumber;
        }

        public void Start()
        {
            InputProcessor.Instance().Play(_playList[0].GetSourceEntity());
        }

        public void Load(String path)
        {
            _playList = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(IsValidVideoExtension)
                .Select(fileName => new PlaylistItem(new FileSourceEntity(fileName)))
                .ToList();
        }

        public List<PlaylistItem> GetPlaylist()
        {
            return _playList;
        }

        public void PlayItemWithNumber(int num)
        {
            _currentSongNumber = num;
            InputProcessor.Instance().Stop();
        }

        private static MediaLibrary _instance;

        public static MediaLibrary instance()
        {
            if (_instance == null)
            {
                _instance = new MediaLibrary();
            }

            return _instance;
        }
    }
}