using System;

namespace func.brainfuck;

public interface IVirtualMachine
{
	void RegisterCommand(char symbol, Action<IVirtualMachine> execute);
	string Instructions { get; }
	int InstructionPointer { get; set; }
	byte[] Memory { get; }
	int MemoryPointer { get; set; }
	void Run();
}