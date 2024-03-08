using System;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;
using System.Runtime.CompilerServices;

namespace StructBenchmarking
{
    public class Benchmark : IBenchmark
    {
        public double MeasureDurationInMs(ITask task, int repetitionCount)
        {
            task.Run();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < repetitionCount; i++)
            {
                task.Run();
            }
            stopwatch.Stop();

            return stopwatch.Elapsed.TotalMilliseconds / repetitionCount;
        }
    }

    public class BuilderTest : ITask
    {
        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        public void Run()
        {
            var str = new StringBuilder();
            for (int i = 0; i < 10000; i++)
            {
                str.Append("a");
            }
            var ans = str.ToString();
        }
    }

    public class StringTest : ITask
    {
        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        public void Run()
        {
            var ans = new string('a', 10000);
        }
    }

    [TestFixture]
    public class RealBenchmarkUsageSample
    {
        [Test]
        public void StringConstructorFasterThanStringBuilder()
        {
            var benchmark = new Benchmark();

            var firstTest = benchmark.MeasureDurationInMs(new StringTest(), 10000);
            var secondTest = benchmark.MeasureDurationInMs(new BuilderTest(), 10000);

            Assert.Less(firstTest, secondTest);
        }
    }
}