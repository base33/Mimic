using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Mimic.Benchmarks.Base;
using Mimic.PropertyMapperAttributes;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models.PublishedContent;

namespace Mimic.Benchmarks
{
    [SimpleJob(RuntimeMoniker.Net472, baseline: true)]
    [RPlotExporter]
    public class MimicVsModelsBuilder : UmbracoBaseTest
    {
        public IPublishedContent TestContent { get; set; }

        [GlobalSetup]
        public override void SetUp()
        {
            base.SetUp();

            var parent = new Mock<IPublishedContent>();
            var parentPropertyList = new List<IPublishedProperty>();
            SetupPropertyValue(parent, "firstName", "Bob", parentPropertyList);
            SetupPropertyValue(parent, "lastName", "Smith", parentPropertyList);
            SetupPropertyValue(parent, "age", 14, parentPropertyList);
            SetupPropertyValue(parent, "webSiteUrl", "https://www.google.com", parentPropertyList);
            parent.Setup(e => e.Properties).Returns(parentPropertyList);

            var children = new List<IPublishedContent>();

            var child1 = new Mock<IPublishedContent>();
            var child1PropertyList = new List<IPublishedProperty>();
            SetupPropertyValue(child1, "firstName", "Bob", child1PropertyList);
            SetupPropertyValue(child1, "lastName", "Smith", child1PropertyList);
            SetupPropertyValue(child1, "age", 14, child1PropertyList);
            SetupPropertyValue(child1, "webSiteUrl", "https://www.google.com", child1PropertyList);
            child1.Setup(e => e.Properties).Returns(child1PropertyList);

            var child2 = new Mock<IPublishedContent>();
            var child2PropertyList = new List<IPublishedProperty>();
            SetupPropertyValue(child2, "firstName", "Bob", child2PropertyList);
            SetupPropertyValue(child2, "lastName", "Smith", child2PropertyList);
            SetupPropertyValue(child2, "age", 14, child2PropertyList);
            SetupPropertyValue(child2, "webSiteUrl", "https://www.google.com", child2PropertyList);
            child2.Setup(e => e.Properties).Returns(child2PropertyList);

            children.Add(child1.Object);
            children.Add(child2.Object);
            parent.Setup(p => p.Children).Returns(children);

            TestContent = parent.Object;
        }

        [Benchmark]
        public void MimicConversion()
        {
            for(var i = 0; i < 100; i++)
            {
                TestContent.As<Person>();
            }
        }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string WebsiteUrl { get; set; }

        [Children]
        public List<Person> Children { get; set; }
    }
}