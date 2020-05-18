using FastMember;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models.PublishedContent;

namespace Mimic.Context
{
    public class MapperContext
    {
        public Member Property { get; internal set; }
        public IPublishedElement Content { get; set; }
    }
}
