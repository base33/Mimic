using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Mimic.PropertyMapperAttributes
{
    public class Ignore : PropertyMapperAttribute
    {
        public override object ProcessValue()
        {
            return null;
        }
    }
}
