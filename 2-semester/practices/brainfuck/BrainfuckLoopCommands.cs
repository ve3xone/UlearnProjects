using System.Collections.Generic;

namespace func.brainfuck;

public class BrainfuckLoopCommands
{
    private static Dictionary<int, int> bracket = new ();
    private static Stack<int> stack = new ();

    public static void RegisterTo(IVirtualMachine vm)
    {
        InitializeBrackets(vm.Instructions);

        vm.RegisterCommand('[', b =>
        {
            if (b.Memory[b.MemoryPointer] == 0)
                b.InstructionPointer = bracket[b.InstructionPointer];
        });
        vm.RegisterCommand(']', b =>
        {
            if (b.Memory[b.MemoryPointer] != 0)
                b.InstructionPointer = bracket[b.InstructionPointer];
        });
    }

    private static void InitializeBrackets(string instructions)
    {
        for (var i = 0; i < instructions.Length; i++)
        {
            if (instructions[i] == '[') 
                stack.Push(i);
            else if (instructions[i] == ']')
            {
                bracket[i] = stack.Peek();
                bracket[stack.Pop()] = i;
            }
        }
    }
}