using System;

namespace func.brainfuck;

public static class BrainfuckBasicCommands
{
    public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
    {
        vm.RegisterCommand('.', (machine) => write((char)machine.Memory[machine.MemoryPointer]));
        vm.RegisterCommand(',', (machine) => { machine.Memory[machine.MemoryPointer] = (byte)read(); });

        vm.RegisterCommand('+', (machine) => machine.Memory[machine.MemoryPointer] = (byte)ModuleSum(machine.Memory[machine.MemoryPointer], 1, 256));
        vm.RegisterCommand('-', (machine) => machine.Memory[machine.MemoryPointer] = (byte)ModuleSum(machine.Memory[machine.MemoryPointer], -1, 256));
        vm.RegisterCommand('>', (machine) => machine.MemoryPointer = ModuleSum(machine.MemoryPointer, 1, machine.Memory.Length));
        vm.RegisterCommand('<', (machine) => machine.MemoryPointer = ModuleSum(machine.MemoryPointer, -1, machine.Memory.Length));

        RegisterCommandForSymbolRange(vm, 'A', 'Z');
        RegisterCommandForSymbolRange(vm, 'a', 'z');
        RegisterCommandForSymbolRange(vm, '0', '9');
    }

    private static int ModuleSum(int current, int shift, int module) => (current + shift + module) % module;

    private static void RegisterCommandForSymbolRange(IVirtualMachine vm, char start, char end)
    {
        for (char c = start; c <= end; c++)
        {
            char tmp = c;
            vm.RegisterCommand(tmp, (machine) => machine.Memory[machine.MemoryPointer] = (byte)tmp);
        }
    }
}