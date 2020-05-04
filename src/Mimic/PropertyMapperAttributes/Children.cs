using Mimic.Factory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models.PublishedContent;

namespace Mimic.PropertyMapperAttributes
{
    public class Children : PropertyMapperAttribute
    {
        public override object ProcessValue()
        {
            if (Context.Property.Type.GenericTypeArguments[0] == typeof (IPublishedContent))
            {
                return Content.Children.ToList();
            }

            IList list = (IList)MsilBuilderWithCachingWithGeneric.Build(Context.Property.Type);
            foreach (var child in Content.Children)
            {
                var typedChild = MimicAsDynamicCaller.GetAsForType(Context.Property.Type.GenericTypeArguments[0])(child);
                list.Add(typedChild);
            }


            return list;
        }
    }
}
