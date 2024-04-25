using System;
using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;
using NUnit.Framework;

namespace Greedy.Tests;

[TestFixture]
public class DijkstraPathFinder_Should
{
    private DijkstraPathFinder pathFinder;

    [SetUp]
    public void Init()
    {
        pathFinder = new DijkstraPathFinder();
    }

    // Формат описания лабиринтов в тестах смотрите в файле states-for-students\Readme.md 
    // Там же находятся все лабиринты, использующиеся при запуске приложения.
    // goal и energy в этой задаче не используются.
    [TestCase(@"202
|
goal / energy
1 2000
|
position
0 0
|
2 0", TestName = "Small Test No Path Horizontal")]
    [TestCase(@"2
0
2
|
goal / energy
1 2000
|
position
0 0
|
0 2", TestName = "Small Test No Path Vertical")]
    [TestCase(@"202
202
202
|
goal / energy
1 2000
|
position
0 0
|
2 2", TestName = "No Path Sqr Map")]
    [TestCase(@"2021
2022
2120
2101
|
goal / energy
1 2000
|
position
0 0
|
3 3", TestName = "No Path After Some Steps")]
    [TestCase(@"11111
10001
10101
10001
11111
|
goal / energy
1 2000
|
position
2 2
|
4 3", TestName = "No Path Start From Center")]
    [TestCase(@"11111
10001
10101
10001
11111
|
goal / energy
4 2000
|
position
2 2
|
4 3
1 4
0 3
2 0", TestName = "No Path To Many Targets")]
    public void Find_Empty_Path_When_No_Path(string map)
    {
        var state = StatesLoader.LoadStateFromInputData(map);
        var paths = pathFinder.GetPathsByDijkstra(state, state.Position, state.Chests).ToList();
        CollectionAssert.AreEqual(new List<PathWithCost>(), paths);
    }

    [TestCase(@"
111
111
111
|
goal / energy
1 4
|
position
0 0
|
0 0", new[] { 0 }, TestName = "Path To Start Position")]
    [TestCase(@"
11
|
goal / energy
1 4
|
position
0 0
|
1 0", new[] { 1 }, TestName = "Go Left")]
    [TestCase(@"
11
|
goal / energy
1 4
|
position
1 0
|
0 0", new[] { 1 }, TestName = "Go Right")]
    [TestCase(@"
1
1
|
goal / energy
1 4
|
position
0 0
|
0 1", new[] { 1 }, TestName = "Go Down")]
    [TestCase(@"
1
1
|
goal / energy
1 4
|
position
0 1
|
0 0", new[] { 1 }, TestName = "Go Up")]
    [TestCase(@"
111
111
111
|
goal / energy
1 4
|
position
0 0
|
2 2", new[] { 4 }, TestName = "Find Any Path")]
    [TestCase(@"
1411
2621
5519
1311
|
goal / energy
1 10
|
position
0 0
|
3 3", new[] { 10 }, TestName = "Find Shortest Path")]
    [TestCase(@"
1411
2621
5519
1311
|
goal / energy
4 10
|
position
0 0
|
3 3
2 2
0 2
2 0", new[] { 10, 8, 7, 5 }, TestName = "Find Paths To All Targets")]
    [TestCase(@"
14112412641274
26211394234886
55194756238523
13112342355642
23243566543566
12345666345555
32452523562122
23435345342345
23434534646643
32434546546763
12334111921111
13423563334556
|
goal / energy
10 10
|
position
5 8
|
3 3
13 10
8 7
3 5
5 10
10 5
11 2
10 9
6 4
1 5", new[] { 21, 23, 15, 19, 6, 28, 45, 26, 21, 21 }, TestName = "Big Field")]
    public void Find_Path_When_Paths_Exists(string inputData, IEnumerable<int> expectedCosts)
    {
        PerformTest(inputData, expectedCosts);
    }

    [TestCase("maze_1", new[] { 180, 132, 242, 226, 167, 207, 216, 261, 107, 185, 262, 147, 241, 230, 54, 263 })]
    [TestCase("maze_2", new[] { 170, 408, 451, 428, 598, 603, 647, 574, 521, 507, 581, 394, 12, 160, 480, 459 })]
    [TestCase("maze_3", new[] { 659, 405, 510, 277, 516, 655, 557, 602, 263, 452, 105, 493, 41, 97, 556, 125 })]
    [TestCase("maze_4", new[] { 25, 32, 39, 46, 53, 60, 67, 74, 81 })]
    public void Find_Shortest_Path_To_Targets_In_Maze(string stateName, IEnumerable<int> expectedCosts)
    {
        // Описание лабиринтов для этого теста можно посмотреть в соответствующих файлах в директории /states-for-students
        var inputData = TestsHelper.ReadFile(stateName);
        PerformTest(inputData, expectedCosts);
    }

    public void PerformTest(string inputData, IEnumerable<int> expectedCosts)
    {
        var state = StatesLoader.LoadStateFromInputData(inputData);
        var costs = state.Chests.Zip(expectedCosts, Tuple.Create)
            .ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
        const int energy = int.MaxValue;
        var start = state.Position;
        var paths = pathFinder.GetPathsByDijkstra(state, state.Position, state.Chests);
        var previousCost = 0;
        foreach (var path in paths)
        {
            state.Energy = energy;
            state.Position = start;
            var controller = TestsHelper.MoveThroughPath(true, state, path.Path.Skip(1).ToList());
            var position = controller.State.Position;
            Assert.True(costs.ContainsKey(position));
            Assert.AreEqual(costs[position], path.Cost);
            Assert.AreEqual(costs[position], energy - controller.State.Energy);
            CollectionAssert.Contains(state.Chests, position);
            Assert.True(previousCost <= path.Cost);
            previousCost = path.Cost;
            costs.Remove(position);
        }

        CollectionAssert.IsEmpty(costs, "Have no path to chests: " + string.Join(";", costs.Keys));
    }

    [Test]
    public void Dijkstra_Path_Finder_Is_Lazy()
    {
        var inputData = TestsHelper.ReadFile("maze_time_limit");
        var state = StatesLoader.LoadStateFromInputData(inputData);
        state.Chests.Add(new Point(1, 1));
        var start = new Point(0, 1);
        Assume.That(!state.IsWallAt(start));
        BasePerformanceTests.RunWithTimeout(() =>
        {
            for (int i = 0; i < 100000; ++i)
            {
                state.Position = start;
                var paths = pathFinder
                    .GetPathsByDijkstra(state, start,
                        state.Chests).FirstOrDefault();
                Assert.IsNotNull(paths);
            }

            return 0;
        }, 2000, "Метод должен быть ленивым!");
    }
}