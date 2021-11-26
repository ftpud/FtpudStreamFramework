using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FtpudStreamFramewok.Core;
using FtpudStreamFramewok.Source;

namespace StreamControlLite.Extensions
{
    public class MediaLibrary
    {
        private List<FileSourceEntity> _fileList;

        private bool IsValidVideoExtension(string filename)
        {
            return filename.EndsWith(".ts")
                   || filename.EndsWith(".mp4")
                   || filename.EndsWith(".mkv")
                   || filename.EndsWith(".m2ts");
        }

        public void Init()
        {
            int listNum = 0;
            InputProcessor.Instance().OnFinished += (sender, eventArgs) =>
            {
                listNum++;
                if (listNum == _fileList.Count) listNum = 0;
                InputProcessor.Instance().Play(_fileList[listNum]);
            };
        }

        public void Start()
        {
            InputProcessor.Instance().Play(_fileList[0]);
        }

        public void Load(String path)
        {
            _fileList = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(IsValidVideoExtension)
                .Select(fileName => new FileSourceEntity(fileName))
                .ToList();
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