using System;
using System.Collections.Generic;
using System.Linq;

namespace FtpudStreamFramewok.Settings.Filters
{

    public class Decorator
    {
        private List<VideoFilter> _globalFilterList = new List<VideoFilter>();
        
        private List<VideoFilter> _filterList = new List<VideoFilter>();

        public void AddGlobalFilter(VideoFilter filter)
        {
            _globalFilterList.Add(filter);
        }
        
        public void WithFilters(VideoFilter[] filters)
        {
            _filterList = new List<VideoFilter>(filters);
        }

        public string GetCommandLine()
        {
            return String.Join(",", _globalFilterList.Union(_filterList).Select(f => f.GetFilterCommandLine()));
        }


        private static Decorator _instance;
        public static Decorator instance()
        {
            if (_instance == null)
            {
                _instance = new Decorator();
            }

            return _instance;
        }
    }
}