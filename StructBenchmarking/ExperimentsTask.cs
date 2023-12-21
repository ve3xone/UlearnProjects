using System;
using System.Collections.Generic;

namespace StructBenchmarking
{
    public interface IStructAndArrayBenchmarks
    {
        ITask RunStructBenchmark(int repetitionsCount);
        ITask RunClassBenchmark(int repetitionsCount);
    }

    public class StructArrayTask : IStructAndArrayBenchmarks
    {
        public ITask RunStructBenchmark(int repetitionsCount)
        {
            return new StructArrayCreationTask(repetitionsCount);
        }

        public ITask RunClassBenchmark(int repetitionsCount)
        {
            return new ClassArrayCreationTask(repetitionsCount);
        }
    }

    public class StructMethodCallTask : IStructAndArrayBenchmarks
    {
        public ITask RunStructBenchmark(int repetitionsCount)
        {
            return new MethodCallWithStructArgumentTask(repetitionsCount);
        }

        public ITask RunClassBenchmark(int repetitionsCount)
        {
            return new MethodCallWithClassArgumentTask(repetitionsCount);
        }
    }

    public class Experiments
    {
        public static ChartData BuildChartDataForArrayCreation(IBenchmark benchmark, int repetitionsCount)
        {
            return RunExperiment(new StructArrayTask(), benchmark, repetitionsCount);
        }

        public static ChartData BuildChartDataForMethodCall(IBenchmark benchmark, int repetitionsCount)
        {
            return RunExperiment(new StructMethodCallTask(), benchmark, repetitionsCount);
        }

        private static ChartData RunExperiment(IStructAndArrayBenchmarks factory,
            IBenchmark benchmark, int repetitionsCount)
        {
            var classesTimes = MeasureExperiment(factory.RunClassBenchmark, benchmark, repetitionsCount);
            var structuresTimes = MeasureExperiment(factory.RunStructBenchmark, benchmark, repetitionsCount);

            return new ChartData
            {
                Title = "Experiment is successful",
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }

        private static List<ExperimentResult> MeasureExperiment(Func<int, ITask> taskFactory,
            IBenchmark benchmark, int repetitionsCount)
        {
            var times = new List<ExperimentResult>();

            foreach (var objectSize in Constants.FieldCounts)
            {
                var testResult = benchmark.MeasureDurationInMs(taskFactory(objectSize), repetitionsCount);
                times.Add(new ExperimentResult(objectSize, testResult));
            }

            return times;
        }
    }
}