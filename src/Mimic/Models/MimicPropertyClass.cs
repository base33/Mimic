using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mimic.Models
{
    public class MimicGeneratedClass
    {
        public MimicGeneratedClass()
        {
            Compositions = new List<MimicGeneratedClass>();
            PropertyTypes = new List<MimicPropertyType>();
        }

        public string ClassName { get; set; }
        public string InterfaceName
        {
            get
            {
                return "I" + ClassName;
            }
        }
        public List<MimicGeneratedClass> Compositions { get; set; }
        public MimicGeneratedClass ParentClass { get; set; }
        public List<MimicPropertyType> PropertyTypes { get; set; }

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
