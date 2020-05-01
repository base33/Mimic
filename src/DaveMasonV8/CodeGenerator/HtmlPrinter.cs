using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaveMasonV8.CodeGenerator
{
    public static class HtmlPrinter
    {
        private enum CSSClass
        {
            STD,
            TYP,
            KWD
        };

        public static string Space
        {
            get
            {
                return string.Format("<span class=\"dm-{0}\"> </span>", CSSClass.STD.ToString().ToLower());
            }
        }
        public static string NewLine
        {
            get
            {
                return "<br>";
            }
        }

        public static string Keyword(string keyword, bool includeSpace = true)
        {
            return string.Format("<span class=\"dm-{0}\">{1}</span>", CSSClass.KWD.ToString().ToLower(), keyword) + (includeSpace ? Space : string.Empty);
        }

        public static string Type(string type, bool includeSpace = true)
        {
            return string.Format("<span class=\"dm-{0}\">{1}</span>", CSSClass.TYP.ToString().ToLower(), type) + (includeSpace ? Space : string.Empty);
        }

        public static string Standard(string word, bool includeSpace = true)
        {

            return string.Format("<span class=\"dm-{0}\">{1}</span>", CSSClass.STD.ToString().ToLower(), word) + (includeSpace ? Space : string.Empty);
        }



    }
}
