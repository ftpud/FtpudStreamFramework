using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FtpudStreamFramework.Core;
using FtpudStreamFramework.Source;
using StreamControlLite.Extensions.Model;

namespace StreamControlLite.Extensions
{
    public class MediaLibrary
    {
        private List<PlaylistItem> _playList;

        private bool IsValidVideoExtension(string filename)
        {
            return filename.EndsWith(".webm")
                   || filename.EndsWith(".flv")
                   || filename.EndsWith(".ts")
                   || filename.EndsWith(".mp4")
                   || filename.EndsWith(".mkv")
                   || filename.EndsWith(".m2ts");
        }
        private Random _random = new Random();

        private int _currentSongNumber = 0;

        public void Init()
        {

            InputProcessor.Instance().OnFinished += (sender, eventArgs) =>
            {
                _currentSongNumber++;
                if (_currentSongNumber == _playList.Count) _currentSongNumber = 0;

                var name = Path.GetFileName(_playList[_currentSongNumber].FileName);
                Extensions.InfoBox.instance().Push(name);
                
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
                //.OrderBy(fileName => fileName.FileName)
                //.ThenBy(num => ExtractNumber(num.FileName))
                .Shuffle(_random)
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
        
        static int ExtractNumber(string text)
        {
            Match match = Regex.Match(text, @"(\d+)");
            if (match == null)
            {
                return 0;
            }

            int value;
            if (!int.TryParse(match.Value, out value))
            {
                return 0;
            }

            return value;
        }
    }
    
    internal static class ArrayHelper
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            var elements = source.ToArray();
            for (var i = elements.Length - 1; i >= 0; i--)
            {
                // Swap element "i" with a random earlier element it (or itself)
                // ... except we don't really need to swap it fully, as we can
                // return it immediately, and afterwards it's irrelevant.
                var swapIndex = rng.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
        }
    }
}