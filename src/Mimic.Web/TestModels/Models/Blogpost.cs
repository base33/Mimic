using Mimic.Web.TestModels.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mimic.Web.TestModels.Models
{
    public class Blogpost : INavigationSEO
    {
        [Required]
        public string Excerpt { get; set; }

        [Required]
        public string PageTitle { get; set; }

        public IEnumerable<string> Categories { get; set; }
        public string BodyText { get; set; }
        public string SeoMetaDescription { get; set; }
        public bool UmbracoNavihide { get; set; }
        public IEnumerable<string> Keywords { get; set; }
    }
}