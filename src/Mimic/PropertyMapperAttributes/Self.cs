﻿using System;
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
            return null;
            //return Content.As(Context.Property.PropertyType);
        }
    }
}
