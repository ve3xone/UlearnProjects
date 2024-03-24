using System;

namespace func.brainfuck;

public static class BrainfuckBasicCommands
{
    public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
    {
        vm.RegisterCommand('.', (m) => write((char)m.Memory[m.MemoryPointer]));
        vm.RegisterCommand(',', (m) => m.Memory[m.MemoryPointer] = (byte)read());

        vm.RegisterCommand('+', (m) => m.Memory[m.MemoryPointer] = (byte)ModuleSum(m.Memory[m.MemoryPointer], 1, 256));
        vm.RegisterCommand('-', (m) => m.Memory[m.MemoryPointer] = (byte)ModuleSum(m.Memory[m.MemoryPointer], -1, 256));
        vm.RegisterCommand('>', (m) => m.MemoryPointer = ModuleSum(m.MemoryPointer, 1, m.Memory.Length));
        vm.RegisterCommand('<', (m) => m.MemoryPointer = ModuleSum(m.MemoryPointer, -1, m.Memory.Length));

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