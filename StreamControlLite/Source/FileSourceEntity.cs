using System;
using StreamControlLite.Settings;
using StreamControlLite.Util;

namespace StreamControlLite.Source
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
            
            return StreamSettings.WrapInput($" -i \"{_fileName}\" ");
        }
    }
}