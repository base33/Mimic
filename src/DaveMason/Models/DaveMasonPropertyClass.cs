using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaveMason.Models
{
    public class DaveMasonGeneratedClass
    {
        public DaveMasonGeneratedClass()
        {
            Compositions = new List<DaveMasonGeneratedClass>();
            PropertyTypes = new List<DaveMasonPropertyType>();
        }

        public string ClassName { get; set; }
        public string InterfaceName
        {
            get
            {
                return "I" + ClassName;
            }
        }
        public List<DaveMasonGeneratedClass> Compositions { get; set; }
        public DaveMasonGeneratedClass ParentClass { get; set; }
        public List<DaveMasonPropertyType> PropertyTypes { get; set; }

        public IEnumerable<string> InheritedClassNames
        {
            get
            {
                return Compositions.Select(composition => composition.ClassName.Replace(" ", string.Empty));
            }
        }

        public IEnumerable<string> InheritedInterfaceNames
        {
            get
            {
                return Compositions.Select(composition => "I" + composition.ClassName.Replace(" ", string.Empty));
            }
        }
    }
}
