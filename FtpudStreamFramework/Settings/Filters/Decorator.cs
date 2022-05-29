using System;
using System.Collections.Generic;
using System.Linq;

namespace FtpudStreamFramework.Settings.Filters
{

    public class Decorator
    {
        private List<VideoFilter> _globalFilterList = new List<VideoFilter>();
        
        private List<VideoFilter> _filterList = new List<VideoFilter>();

        public void AddGlobalFilter(VideoFilter filter)
        {
            _globalFilterList.Add(filter);
        }
        
        public Decorator WithFilters(VideoFilter[] filters)
        {
            _filterList = new List<VideoFilter>(filters);
            return this;
        }

        public Decorator AppendFilter(VideoFilter filter)
        {
            _filterList.Add(filter);
            return this;
        }

        public string GetCommandLine()
        {
            return String.Join(",", _globalFilterList.Union(_filterList).Where(f => f != null).Select(f => f.GetFilterCommandLine()));
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