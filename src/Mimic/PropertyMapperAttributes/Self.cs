using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mimic.PropertyMapperAttributes
{
    public class Self : PropertyMapperAttribute
    {
        public override object ProcessValue()
        {
            return Factory.MimicAsDynamicCaller.GetAsForType(Context.Property.Type)(Context.Content);
        }
    }
}
