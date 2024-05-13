using NUnit.Framework;
using System.Collections.Generic;

namespace DiskTree;

[TestFixture]
public class DiskTreeTask_should
{

	public void MakeTest(List<string> input, List<string> answer)
	{
		var result = DiskTreeTask.Solve(input);
		CollectionAssert.AreEqual(answer, result);
	}

	[Test]
	public void Test1() { MakeTest(new List<string> { @"WINNT\SYSTEM32\CONFIG", @"GAMES", @"WINNT\DRIVERS", @"HOME", @"WIN\SOFT", @"GAMES\DRIVERS", @"WINNT\SYSTEM32\CERTSRV\CERTCO~1\X86", }, new List<string> { @"GAMES", @" DRIVERS", @"HOME", @"WIN", @" SOFT", @"WINNT", @" DRIVERS", @" SYSTEM32", @"  CERTSRV", @"   CERTCO~1", @"    X86", @"  CONFIG", }); }

	[Test]
	public void Test2() { MakeTest(new List<string> { @"USERS", }, new List<string> { @"USERS", }); }

	[Test]
	public void Test3() { MakeTest(new List<string> { @"A", @"B", @"C", @"D", @"E", @"F", @"G", }, new List<string> { @"A", @"B", @"C", @"D", @"E", @"F", @"G", }); }

	[Test]
	public void Test4() { MakeTest(new List<string> { @"!#$%&'()\-@^_`{}~\!#$%&'()\-@^_`{}~\!#$%&'()\-@^_`{}~\!#$%&'()\-@^_`{}~\!#$%&'()", }, new List<string> { @"!#$%&'()", @" -@^_`{}~", @"  !#$%&'()", @"   -@^_`{}~", @"    !#$%&'()", @"     -@^_`{}~", @"      !#$%&'()", @"       -@^_`{}~", @"        !#$%&'()", }); }

	[Test]
	public void Test5() { MakeTest(new List<string> { @"AAAA", @"AAAA\AAAA", @"AAA\AA\AAA", @"A\AAA\AAA", @"AA\A\AAAA", @"AAA\A\AAAA", @"AA\AAA", @"A\A\A\A", @"AA\AA\AA\AA", @"AAA\AAA\AAA\AAA", @"AAAA\AAAA\AAAA\AAAA", @"AA\AA\AA", @"A\AA\A\AA", }, new List<string> { @"A", @" A", @"  A", @"   A", @" AA", @"  A", @"   AA", @" AAA", @"  AAA", @"AA", @" A", @"  AAAA", @" AA", @"  AA", @"   AA", @" AAA", @"AAA", @" A", @"  AAAA", @" AA", @"  AAA", @" AAA", @"  AAA", @"   AAA", @"AAAA", @" AAAA", @"  AAAA", @"   AAAA", }); }

}