using System.Collections.Generic;

namespace StructBenchmarking
{
    public interface IBenchmarkTasks
    {
        ITask CreateTaskForStruct(int repetitionsCount);
        ITask CreateTaskForClass(int repetitionsCount);
    }

    public class ArrayCreationTasks : IBenchmarkTasks
    {
        public ITask CreateTaskForStruct(int repetitionsCount) =>
            new StructArrayCreationTask(repetitionsCount);

        public ITask CreateTaskForClass(int repetitionsCount) =>
            new ClassArrayCreationTask(repetitionsCount);
    }

    public class MethodCallTasks : IBenchmarkTasks
    {
        public ITask CreateTaskForStruct(int repetitionsCount) =>
            new MethodCallWithStructArgumentTask(repetitionsCount);

        public ITask CreateTaskForClass(int repetitionsCount) =>
            new MethodCallWithClassArgumentTask(repetitionsCount);
    }

    public class Experiments
    {
        public static ChartData BuildChartDataForArrayCreation(IBenchmark benchmark, int repetitionsCount)
        {
            var classesTimes = new List<ExperimentResult>();
            var structuresTimes = new List<ExperimentResult>();

            foreach (var objectSize in Constants.FieldCounts)
            {
                classesTimes.Add(new ExperimentResult
                    (objectSize, benchmark.MeasureDurationInMs
                        (new ArrayCreationTasks().CreateTaskForClass(objectSize), repetitionsCount)));
                structuresTimes.Add(new ExperimentResult
                    (objectSize, benchmark.MeasureDurationInMs
                        (new ArrayCreationTasks().CreateTaskForStruct(objectSize), repetitionsCount)));
            }

            return new ChartData
            {
                Title = "Create array",
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }

        public static ChartData BuildChartDataForMethodCall(IBenchmark benchmark, int repetitionsCount)
        {
            var classesTimes = new List<ExperimentResult>();
            var structuresTimes = new List<ExperimentResult>();

            foreach (var objectSize in Constants.FieldCounts)
            {
                classesTimes.Add(new ExperimentResult
                    (objectSize, benchmark.MeasureDurationInMs
                        (new MethodCallTasks().CreateTaskForClass(objectSize), repetitionsCount)));
                structuresTimes.Add(new ExperimentResult
                    (objectSize, benchmark.MeasureDurationInMs
                        (new MethodCallTasks().CreateTaskForStruct(objectSize), repetitionsCount)));
            }

            return new ChartData
            {
                Title = "Call method with argument",
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }
    }
}