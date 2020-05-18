using Mimic.PropertyMapperAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models.PublishedContent;

namespace Mimic.Web.TestModels.Models
{
    public class Everything
    {
        public bool Checkbox { get; set; }
        public Everything ContentPicker { get; set; }
        public DateTime DateAndTime { get; set; }
        public object Maps { get; set; }
        public IEnumerable<IPublishedElement> MultinodeTree { get; set; }
        public decimal NumberDec { get; set; }

        public IEnumerable<Feature> Features { get; set; }

        [Children]
        public List<Blogpost> Children { get; set; }
    }
}