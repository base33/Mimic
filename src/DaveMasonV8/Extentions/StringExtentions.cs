using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaveMasonV8
{
    public static class StringExtentions
    {
        public static string ReplaceFirstOccurrence(this string str, string find, string replace)
        {
            int place = str.IndexOf(find);

            if (place == -1)
                return str;

            string result = str.Remove(place, find.Length).Insert(place, replace);
            return result;
        }
        public static string ReplaceLastOccurrence(this string str, string find, string replace)
        {
            int place = str.LastIndexOf(find);

            if (place == -1)
                return str;

            string result = str.Remove(place, find.Length).Insert(place, replace);
            return result;
        }
    }
}
