using System;
using System.Collections.Generic;
using System.Linq;

namespace FtpudStreamFramewok.Settings.Filters
{
    public class TextFilterOption
    {
        public enum OptionName
        {
            text,
            fontcolor,
            fontfile,
            fontsize,
            x,
            y,
            boxcolor,
            boxborderw,
            textfile,
            box,
            reload
        }

        public OptionName option { get; }
        public String value { get; }

        public TextFilterOption(OptionName option, String value)
        {
            this.option = option;
            this.value = value;
        }
    }

    public class TextFilter : VideoFilter
    {
        private TextFilterOption[] _optionList;

        public TextFilter(TextFilterOption[] optionList)
        {
            _optionList = optionList;
        }

        public override string GetFilterCommandLine()
        {
            return "drawtext=" + String.Join(":",
                _optionList.Select(option =>
                    $"{Enum.GetName(option.option.GetType(), option.option)}='{option.value.Replace("'", "\'")}'"));
        }
    }
}