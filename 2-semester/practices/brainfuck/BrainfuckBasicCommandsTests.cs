using NUnit.Framework;

namespace func.brainfuck;

[TestFixture]
public class BrainfuckBasicCommandsTests
{
	[Test]
	public void Print()
	{
		Assert.AreEqual("\0", Run("."));
	}

	[Test]
	public void Inc()
	{
		Assert.AreEqual("\x1", Run("+."));
		Assert.AreEqual("\x5", Run("+++++."));
		Assert.AreEqual("A", Run(new string('+', 'A') + "."));
		Assert.AreEqual("Z", Run(new string('+', 'Z') + "."));
		Assert.AreEqual("\xFF", Run(new string('+', 255) + "."));
	}

	[Test]
	public void Dec()
	{
		Assert.AreEqual("\0", Run("+-."));
		Assert.AreEqual("\x1", Run("+++--."));
		Assert.AreEqual("A", Run(new string('+', 'C') + "--."));
	}

	[Test]
	public void IncOverflow()
	{
		Assert.AreEqual("\x1", Run(new string('+', 257) + "."));
	}

	[Test]
	public void DecOverflow()
	{
		Assert.AreEqual("\xFF", Run("-."));
		Assert.AreEqual("\x1", Run(new string('-', 255) + "."));
	}

	[Test]
	public void Shift()
	{
		Assert.AreEqual(3, Vm(">>>").MemoryPointer);
		Assert.AreEqual(2, Vm(">>><").MemoryPointer);
		Assert.AreEqual(1, Vm(">>><<").MemoryPointer);
		Assert.AreEqual(0, Vm("><").MemoryPointer);
		Assert.AreEqual("\x2", Run("+>++."));
		Assert.AreEqual("\x1", Run("+>++<."));
		Assert.AreEqual("\x1", Run("+>++>+++<<."));
	}

	[Test]
	public void ShiftOverflow()
	{
		Assert.AreEqual(2, Vm("<", 3).MemoryPointer);
		Assert.AreEqual(0, Vm("<>", 3).MemoryPointer);
		Assert.AreEqual(0, Vm(">>>", 3).MemoryPointer);
		Assert.AreEqual(1, Vm(">>>>", 3).MemoryPointer);
		Assert.AreEqual("\x1", Run("<+."));
		Assert.AreEqual("\x2", Run("++<>."));
		Assert.AreEqual("\x1", Run("+<++<+++>>."));
		Assert.AreEqual("\x3", Run("+++" + new string('>', 30000) + "."));
		Assert.AreEqual("\x3", Run("+++" + new string('<', 30000) + "."));
	}

	[Test]
	public void Read()
	{
		Assert.AreEqual("A", Run(",.", "A"));
		Assert.AreEqual("ABC", Run(",.,.,.", "ABC"));
	}

	[Test]
	public void HelloWorld()
	{
		Assert.AreEqual("Hello World!\n", Run(@"
 +++++++++++++++++++++++++++++++++++++++++++++
 +++++++++++++++++++++++++++.+++++++++++++++++
 ++++++++++++.+++++++..+++.-------------------
 ---------------------------------------------
 ---------------.+++++++++++++++++++++++++++++
 ++++++++++++++++++++++++++.++++++++++++++++++
 ++++++.+++.------.--------.------------------
 ---------------------------------------------
 ----.-----------------------.
"));
	}

	[Test]
	public void Constants()
	{
		Assert.AreEqual("QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890",
			Run(
				"Q.W.E.R.T.Y.U.I.O.P.A.S.D.F.G.H.J.K.L.Z.X.C.V.B.N.M." +
				"q.w.e.r.t.y.u.i.o.p.a.s.d.f.g.h.j.k.l.z.x.c.v.b.n.m." +
				"1.2.3.4.5.6.7.8.9.0."));
	}

	[Test]
	public void IgnoreOtehrSymbols()
	{
		Assert.AreEqual("Q", 
			Run("Q!@#$%&*()."));
	}

	[Test]
	public void HelloWorldWithConstants()
	{
		Assert.AreEqual("Hello world",
			Run("H>e>l>l>o>++++++++++++++++++++++++++++++++>w>o>r>l>d<<<<<<<<<<.>.>.>.>.>.>.>.>.>.>."));
	}

	private string Run(string program, string input = "")
	{
		return Brainfuck.Run(program, input);
	}

	private IVirtualMachine Vm(string program, int memorySize = 10)
	{
		var vm = new VirtualMachine(program, memorySize);
		BrainfuckBasicCommands.RegisterTo(vm, () => -1, c => {});
		BrainfuckLoopCommands.RegisterTo(vm);
		vm.Run();
		return vm;
	}
}