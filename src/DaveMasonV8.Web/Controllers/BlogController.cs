using DaveMasonV8.Web.TestModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace DaveMasonV8.Web.Controllers
{
    public class BlogController : RenderMvcController
    {
        public BlogController()
        {
        }

        // GET: Blog
        public ActionResult Index()
        {      
            var blogs = CurrentPage.Children;
            var typedBlog = CurrentPage.As<Everything>();
         
            var breakpoint = typedBlog;            

            return View();
        }
    }
}