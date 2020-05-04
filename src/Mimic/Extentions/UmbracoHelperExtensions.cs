using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Web;

namespace Mimic
{
    public static class UmbracoHelperExtensions
    {
        public static T MappedTypedContent<T>(this UmbracoHelper helper, int id) where T : new()
        {
            return helper.Content(id).As<T>();
        }
    }
}
