using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaveMasonV8.Models
{
    public class DaveMasonPropertyType
    {
        public string PropertyGroup { get; set; }
        public string Name { get; set; }
        public string PropertyType { get; set; }
        public bool IsMandatory { get; set; }
    }
}
