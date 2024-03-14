using System;

namespace func.brainfuck;

public static class BrainfuckBasicCommands
{
    public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
    {
        vm.RegisterCommand('.', (machine) => write((char)machine.Memory[machine.MemoryPointer]));
        vm.RegisterCommand(',', (machine) =>
        {
            int input = read();
            if (input != -1)
            {
                machine.Memory[machine.MemoryPointer] = (byte)input;
            }
        });
        vm.RegisterCommand('+', (machine) => AddToValueInMemory(machine, +1));
        vm.RegisterCommand('-', (machine) => AddToValueInMemory(machine, -1));
        vm.RegisterCommand('>', (machine) => AddToMemoryPointer(machine, +1));
        vm.RegisterCommand('<', (machine) => AddToMemoryPointer(machine, -1));

        RegisterCommandForSymbolRange(vm, 'A', 'Z');
        RegisterCommandForSymbolRange(vm, 'a', 'z');
        RegisterCommandForSymbolRange(vm, '0', '9');
    }

    private static void AddToValueInMemory(IVirtualMachine vm, int value)
    {
        vm.Memory[vm.MemoryPointer] = unchecked((byte)(vm.Memory[vm.MemoryPointer] + value));
    }

    private static void AddToMemoryPointer(IVirtualMachine vm, int value)
    {
        vm.MemoryPointer += value;
        if (vm.MemoryPointer == vm.Memory.Length)
            vm.MemoryPointer = 0;
        else if (vm.MemoryPointer == -1)
            vm.MemoryPointer = vm.Memory.Length - 1;
    }

    private static void RegisterCommandForSymbolRange(IVirtualMachine vm, char start, char end)
    {
        for (char c = start; c <= end; c++)
        {
            char tmp = c;
            vm.RegisterCommand(c, (machine) => machine.Memory[machine.MemoryPointer] = (byte)tmp);
        }
    }
}