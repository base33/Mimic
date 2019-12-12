using DaveMason.Extentions;
using DaveMason.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace DaveMason.Web.Controllers
{
    public class ContentPageController : RenderMvcController
    {
        public override ActionResult Index(RenderModel renderModel)
        {
            var nodeId = 1155;
            var helper = new UmbracoHelper(UmbracoContext.Current);

            var model = helper.MappedTypedContent<BlogpostChild>(nodeId);

            return null;
        }
    }

  





}