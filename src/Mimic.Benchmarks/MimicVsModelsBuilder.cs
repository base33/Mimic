using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Mimic.Benchmarks.Base;
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

            var mock = new Mock<IPublishedContent>();
            SetupPropertyValue(mock, "firstName", "Bob");
            SetupPropertyValue(mock, "lastName", "Smith");
            SetupPropertyValue(mock, "age", 14);
            SetupPropertyValue(mock, "webSiteUrl", "https://www.google.com");

            TestContent = mock.Object;
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
    }
}