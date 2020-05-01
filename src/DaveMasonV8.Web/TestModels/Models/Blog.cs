using DaveMasonV8.Web.TestModels.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Umbraco.Core.Models.PublishedContent;

namespace DaveMasonV8.Web.TestModels.Models
{
    public class Blog : IContent, INavigationSEO
    {
        [Required]
        public decimal HowManyPostsShouldBeShown { get; set; }

        [Required]
        public string PageTitle { get; set; }

        public string DisqusShortname { get; set; }
        public string BodyText { get; set; }
        public bool Checkbox { get; set; }
        public IPublishedElement ContentPicker { get; set; }
        public DateTime DateAndTime { get; set; }
        public object Maps { get; set; }
        public IEnumerable<IPublishedContent> MultinodeTree { get; set; }
        public decimal NumberDec { get; set; }
        public string SeoMetaDescription { get; set; }
        public bool UmbracoNavihide { get; set; }
        public IEnumerable<string> Keywords { get; set; }
    }
}