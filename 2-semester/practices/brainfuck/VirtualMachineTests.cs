using NUnit.Framework;
// ReSharper disable IsExpressionAlwaysTrue

namespace func.brainfuck;

[TestFixture]
public class VirtualMachineTests
{
	[Test]
	public void ImplementIVirtualMachine()
	{
		Assert.IsTrue(new VirtualMachine("", 1) is IVirtualMachine);
	}
	[Test]
	public void Initialize()
	{
		var vm = new VirtualMachine("xxx", 12345);
		vm.RegisterCommand('x', b => { });
		Assert.AreEqual(12345, vm.Memory.Length);
		Assert.AreEqual(0, vm.MemoryPointer);
		Assert.AreEqual("xxx", vm.Instructions);
		Assert.AreEqual(0, vm.InstructionPointer);
	}

	[Test]
	public void SetMemorySize()
	{
		var vm = new VirtualMachine("", 42);
		Assert.AreEqual(42, vm.Memory.Length);
	}

	[Test]
	public void IncrementInstructionPointer()
	{
		var res = "";
		var vm = new VirtualMachine("xy", 10);
		vm.RegisterCommand('x', b => { res += "x->" + b.InstructionPointer + ", "; });
		vm.RegisterCommand('y', b => { res += "y->" + b.InstructionPointer; });
		vm.Run();
		Assert.AreEqual("x->0, y->1", res);
	}

	[Test]
	public void MoveInstructionPointerForward()
	{
		var res = "";
		var vm = new VirtualMachine("xyz", 10);
		vm.RegisterCommand('x', b => { b.InstructionPointer++; });
		vm.RegisterCommand('y', b => { Assert.Fail(); });
		vm.RegisterCommand('z', b => { res += "z"; });
		vm.Run();
		Assert.AreEqual("z", res);
	}
	    
	[Test]
	public void InstructionPointerCanMovedOutside()
	{
		var res = "";
		var vm = new VirtualMachine("yxz", 10);
		vm.RegisterCommand('x', b => { res += "x"; });
		vm.RegisterCommand('y', b => { Assert.Fail(); });
		vm.RegisterCommand('z', b => { res += "z"; });
		vm.InstructionPointer++;
		vm.Run();
		Assert.AreEqual("xz", res);
	}

	[Test]
	public void MoveInstructionPointerBackward()
	{
		var res = "";
		var vm = new VirtualMachine(">><", 10);
		vm.RegisterCommand('>', b =>
		{
			b.InstructionPointer++;
			res += ">";
		});
		vm.RegisterCommand('<', b =>
		{
			b.InstructionPointer -= 2;
			res += "<";
		});
		vm.Run();
		Assert.AreEqual("><>", res);
	}
	[Test]
	public void ChangeMemoryPointer()
	{
		var vm = new VirtualMachine("xy", 10);
		vm.RegisterCommand('x', b => { b.MemoryPointer = b.MemoryPointer + 42; });
		vm.RegisterCommand('y', b => { Assert.AreEqual(42, b.MemoryPointer); });
		vm.Run();
	}

	[Test]
	public void SkipUnknownCommands()
	{
		new VirtualMachine("xyz", 10).Run();
	}

	[Test]
	public void ReadWriteMemory()
	{
		var vm = new VirtualMachine("wr", 10);
		vm.RegisterCommand('w', b => { b.Memory[3] = 42; });
		vm.RegisterCommand('r', b => { Assert.AreEqual(42, b.Memory[3]); });
		vm.Run();
	}

	[Test]
	public void RunInstructionsInRightOrder()
	{
		var vm = new VirtualMachine("abbaaa", 10);
		var res = "";
		vm.RegisterCommand('a', b => { res += "a"; });
		vm.RegisterCommand('b', b => { res += "b"; });
		if (res != "")
			Assert.Fail("Instructions should not be executed before Run() call");
		vm.Run();
		if (res != "")
			Assert.AreEqual("abbaaa", res, "Execution order of program 'abbaaa'");
	}

	[Test]
	public void RunManyInstructions()
	{
		var vm = new VirtualMachine(new string('a', 10000), 10);
		var count = 0;
		vm.RegisterCommand('a', b => { count++; });
		vm.Run();
		Assert.AreEqual(10000, count, "Number of executed instructions");
	}
}