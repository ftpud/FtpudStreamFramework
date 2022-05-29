using System;
using FtpudStreamFramework.Settings;
using FtpudStreamFramework.Settings.Filters;

namespace FtpudStreamFramework.Source
{
    public class SourceEntity
    {
        public String ProvideSourceCommandLine()
        {
            Decorator.instance().WithFilters(new VideoFilter[] { });
            var inputCommand = SetSourceCommand();
            Decorator.instance().AppendFilter(StreamSettings.VideoEncoder.GetPostFilter());
            return CommandLineHelper.WrapInput(inputCommand);
        }

        public virtual String SetSourceCommand()
        {
            throw new NotImplementedException();
        }
    }
}