using BenchmarkDotNet.Running;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mimic.Benchmarks
{
    public static class Program
    {
        public static void Main()
        {
            MimicVsModelsBuilder test = new MimicVsModelsBuilder();
            test.SetUp();
            test.MimicConversion();
            BenchmarkRunner.Run<MimicVsModelsBuilder>();
        }
    }
}