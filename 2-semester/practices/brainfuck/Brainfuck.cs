using System;
using System.Text;

namespace func.brainfuck;

public class Brainfuck
{
	public static string Run(string program, string input = "")
	{
		var inputIndex = 0;
		var output = new StringBuilder();
		Run(program,
			() => inputIndex >= input.Length ? -1 : input[inputIndex++],
			c => output.Append(c));
		return output.ToString();
	}

	public static void Run(string program, Func<int> read, Action<char> write, int memorySize = 30000)
	{
		var vm = new VirtualMachine(program, memorySize);
		BrainfuckBasicCommands.RegisterTo(vm, read, write);
		BrainfuckLoopCommands.RegisterTo(vm);
		vm.Run();
	}
}