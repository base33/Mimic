using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Web;

namespace DaveMasonV8
{
    public static class UmbracoHelperExtensions
    {
        public static T MappedTypedContent<T>(this UmbracoHelper helper, int id)
        {
            return helper.Content(id).As<T>();
        }
    }
}
