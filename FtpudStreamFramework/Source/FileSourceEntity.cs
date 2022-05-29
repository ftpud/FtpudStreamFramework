using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FtpudStreamFramework.Settings;
using FtpudStreamFramework.Settings.Filters;
using FtpudStreamFramework.Util;

namespace FtpudStreamFramework.Source
{

    public class FileSourceEntity : SourceEntity
    {
        public FileSourceEntity(String fileName)
        {
            this._fileName = fileName;
        }
        private string _fileName;

        public String GetFileName()
        {
            return _fileName;
        }

        public override string SetSourceCommand()
        {
            LogUtils.Log(LogLevel.Verbose, $"Playing: {_fileName}");

            var noExtensionName = Path.GetFileNameWithoutExtension(_fileName);
            var fullPath = Path.GetDirectoryName(_fileName);

           /* Decorator.instance().AppendFilter(new TextFilter(new[]
            {
                new TextFilterOption(TextFilterOption.OptionName.text, escapeFfString(Path.GetFileName(_fileName))),
                new TextFilterOption(TextFilterOption.OptionName.fontcolor, "white@0.8"),
                new TextFilterOption(TextFilterOption.OptionName.fontsize, "35"),
                new TextFilterOption(TextFilterOption.OptionName.x, "2"),
                new TextFilterOption(TextFilterOption.OptionName.y, "2"),
                new TextFilterOption(TextFilterOption.OptionName.box, "1"),
                new TextFilterOption(TextFilterOption.OptionName.boxcolor, "black@0.4"),
                new TextFilterOption(TextFilterOption.OptionName.boxborderw, "4"),
                new TextFilterOption(TextFilterOption.OptionName.fontfile,
                    "/usr/share/fonts/truetype/wqy/wqy-microhei.ttc")
            })); */

            var subtitlesFile = new[] { ".", "sub", "Sub", "SUB" }.ToList()
                .Select(d => Path.Combine(fullPath, d, noExtensionName + ".ass"))
                .FirstOrDefault(d => File.Exists(d));
            
            if (subtitlesFile != null)
            {
                LogUtils.Log(LogLevel.Verbose, "Subtitles found: " + subtitlesFile);
                Decorator.instance().AppendFilter(new AssSubtitlesFilter(subtitlesFile));
            }

            return $" -i \"{_fileName}\" ";
        }

        private String escapeFfString(String  input)
        {
            return input
                .Replace("%", "")
                .Replace("'", "");
        }
    }
}