using System.Collections.Generic;

namespace func.brainfuck
{
    public class BrainfuckLoopCommands
    {
        public static void RegisterTo(IVirtualMachine vm)
        {
            var bracket = InitializeBrackets(vm.Instructions);

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

        private static Dictionary<int, int> InitializeBrackets(string instructions)
        {
            var bracket = new Dictionary<int, int>();
            var stack = new Stack<int>();

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

            return bracket;
        }
    }
}