using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models.PublishedContent;

namespace DaveMason.Context
{
    public class MapperContext
    {
        public PropertyInfo Property { get; internal set; }
        public IPublishedContent Content { get; set; }
    }
}
