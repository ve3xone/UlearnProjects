using System;
using System.Collections.Generic;

namespace func.brainfuck;

public class VirtualMachine : IVirtualMachine
{
    public string Instructions { get; }
    public int InstructionPointer { get; set; }
    public byte[] Memory { get; }
    public int MemoryPointer { get; set; }
    private readonly Dictionary<char, Action<IVirtualMachine>> _commands;

    public VirtualMachine(string program, int memorySize)
    {
        Instructions = program;
        Memory = new byte[memorySize];
        _commands = new();
    }

    public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
    {
        _commands.Add(symbol, execute);
    }

    public void Run()
    {
        while (InstructionPointer < Instructions.Length)
        {
            char symbol = Instructions[InstructionPointer];
            if (_commands.TryGetValue(symbol, out var command))
                command.Invoke(this);
            InstructionPointer++;
        }
    }
}