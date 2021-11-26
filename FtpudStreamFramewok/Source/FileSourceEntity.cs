using System;
using System.IO;
using FtpudStreamFramewok.Settings;
using FtpudStreamFramewok.Settings.Filters;
using FtpudStreamFramewok.Util;

namespace FtpudStreamFramewok.Source
{

    public class FileSourceEntity : SourceEntity
    {
        public FileSourceEntity(String fileName)
        {
            this._fileName = fileName;
        }
        private string _fileName;
        
        public override string ProvideSource()
        {
            LogUtils.Log(LogLevel.Verbose, $"Playing: {_fileName}");
            
            Decorator.instance().WithFilters(new []
            {
                new TextFilter(new []
                {
                    new TextFilterOption(TextFilterOption.OptionName.text, Path.GetFileName(_fileName)),
                    new TextFilterOption(TextFilterOption.OptionName.fontcolor, "white@0.8"),
                    new TextFilterOption(TextFilterOption.OptionName.fontsize, "35"),
                    new TextFilterOption(TextFilterOption.OptionName.x, "2"),
                    new TextFilterOption(TextFilterOption.OptionName.y, "2"),
                    new TextFilterOption(TextFilterOption.OptionName.box, "1"),
                    new TextFilterOption(TextFilterOption.OptionName.boxcolor, "black@0.4"),
                    new TextFilterOption(TextFilterOption.OptionName.boxborderw, "4"),
                })
            });
            
            return CommandLineHelper.WrapInput($" -i \"{_fileName}\" ");
        }
    }
}