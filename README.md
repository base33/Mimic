# What is Mimic and why would I use it?
Mimic is a Custom Model content mapper for Umbraco 8 that allows your to map IPublishedContent or ModelsBuilder models to custom class models.  

Using custom models allows for more flexibility when creating smaller granular models that are not to be dictated by Document Type structure or architecture, but also provide simpler ways to extend upon them without the need for partial classes.

## Usage
Here is an example of a class representing a blog post:
```csharp
namespace MyWebsite.Models
{
    public class Blogpost : INavigationSEO
    {
        public string Excerpt { get; set; }
        public string PageTitle { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public string BodyText { get; set; }
        public string SeoMetaDescription { get; set; }
        public bool UmbracoNavihide { get; set; }
        public IEnumerable<string> Keywords { get; set; }
    }
}
```

Here is an example of how you could use Mimic to convert children of the current page into instances of Blogpost:
```csharp
using Mimic;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using MyWebsite.Models;

namespace MyWebsite.Controllers
{
    public class BlogController : RenderMvcController
    {
        // GET: Blog
        public ActionResult Index()
        {      
            var typedBlogs = CurrentPage.Children.As<Blogpost>();

            return View(typedBlogs);
        }
    }
}
```

# How to write a custom property mapper
You may want to map a property from Umbraco and convert it into something custom.  This may be because Umbraco isn't giving you the value in the exact format you'd like, or because Mimic isn't converting it into the type that you want.

To write your own mapper is super simple:
```csharp
public class MyCustomPropertyAttribute : PropertyMapperAttribute
{
    protected int MyCustomValue;

    //constructor with custom inputs from attribute
    public MyCustomPropertyAttribute(int myCustomValue)
    {
        MyCustomValue = myCustomValue;
    }

    public override object ProcessValue()
    {
        //automatically get the closest property 
        //based as the current class property name
        //... or resolve it yourself (passed in via attribute parameter?)
        var property = GetClosestProperty();
        
        //process umbraco property value
        
        return property.Value;
    }
}
```

From within your custom Attribute, you have access to a context that gives access to:
* Content = IPublishedContent
* Property = PropertyInfo for the property on the class this attribute is related to

Now you can use this custom property mapper like so:
```csharp
namespace MyWebsite.Models
{
    public class Blogpost : INavigationSEO
    {
        [MyCustomProperty(100)]
        public string Excerpt { get; set; }
        public string PageTitle { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public string BodyText { get; set; }
        public string SeoMetaDescription { get; set; }
        public bool UmbracoNavihide { get; set; }
        public IEnumerable<string> Keywords { get; set; }
    }
}
```

You will then be able to completely intercept the mapping of this property.  Even better, you can also run this against properties that extend upon Umbraco and do not exist on an Umbraco document type.

# Built-in Property Mapper Attributes
```csharp
[Ignore] // allows you to tell Mimic to ignore this property
[Children] // allows you to populate a list from the children of the current IPublishedContent model
[Self] // allows you to populate a property by applying the current IPublishedContent model to it
```

# The code creation panel!
When you install Mimic, it comes with a dashboard tab located under 'Settings' that can generate the code you can use for specific document types.  This makes it super simple to get the baseline for your classes and then modify them from there.


# How to run the project
Mimic.Web is just a sandbox project used for testing

The credentials for Umbraco are:
Username: admin@admin.com	
Password: qwe12345678

# How different is it to Ditto?
At face value, I built this to be as close to the way we work with Ditto in Umbraco 7.  However, internally I taken my own approach.  Both because Ditto had been causing me issues when previewing content in Umbraco's preview mode, but also it was a learning experience for myself.  But Ditto is the inspiration for this project.

# Contributing or raising issues
Please use the issues tracker within this repository.  If you find a bug, please report it or submit a pull request.

# Feature requests
Please use the issue tracker within this repository.  If you have built any cool custom property mappers that you think will be useful for the community, please feel free to raise an issue and submit a pull request!

# How to contact me
Tweet me at @craignoble1989
