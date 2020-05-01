using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;

namespace DaveMason.Web.Models
{
    public interface INavigationSEO
    {
        string SeoMetaDescription { get; set; }
        bool UmbracoNavihide { get; set; }
        IEnumerable<string> Keywords { get; set; }
    }

    public class Blogpost : INavigationSEO
    {
        [Required]
        public string Excerpt { get; set; }

        [Required]
        public string PageTitle { get; set; }

        public IEnumerable<string> Categories { get; set; }
        public string BodyText { get; set; }
        public IPublishedContent Picker { get; set; }
        //public string Picker { get; set; }
        public string SeoMetaDescription { get; set; }
        public bool UmbracoNavihide { get; set; }
        public IEnumerable<string> Keywords { get; set; }
    }

    public class BlogpostChild : Blogpost
    {
        public string BPCName { get; set; }
        public IEnumerable<NestedContentItem> NestedContentStuff { get; set; }
    }

    public class NestedContentItem
    {
        public string Color { get; set; }
        public IEnumerable<DeepItem> NestedItems { get; set; }
        public string Title { get; set; }
    }

    public class DeepItem
    {
        public string Again { get; set; }
        public string Test { get; set; }
    }
}