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
            var parent = Content as IPublishedContent;

            if (Context.Property.Type.GenericTypeArguments[0] == typeof (IPublishedContent))
            {
                return parent.Children.ToList();
            }

            IList list = (IList)MsilBuilderWithCachingWithGeneric.Build(Context.Property.Type);

            if (list == null)
                return list;

            foreach (var child in parent.Children)
            {
                var typedChild = MimicAsDynamicCaller.GetAsForType(Context.Property.Type.GenericTypeArguments[0])(child);
                list.Add(typedChild);
            }


            return list;
        }
    }
}
