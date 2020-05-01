using Mimic.Web.TestModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace Mimic.Web.Controllers
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

            var typedBlogs = blogs.As<Blogpost>();


            var breakpoint = typedBlog;            

            return View(CurrentPage);
        }
    }
}