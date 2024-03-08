using Avalonia.Input;
using Digger.Architecture;

namespace Digger;

class Player : ICreature
{
    public CreatureCommand Act(int x, int y)
    {
        CreatureCommand diggerCommand = new();

        switch (Game.KeyPressed)
        {
            case Key.Right when x + 1 < Game.MapWidth:
                diggerCommand.DeltaX++;
                break;
            case Key.Left when x - 1 >= 0:
                diggerCommand.DeltaX--;
                break;
            case Key.Down when y + 1 < Game.MapHeight:
                diggerCommand.DeltaY++;
                break;
            case Key.Up when y - 1 >= 0:
                diggerCommand.DeltaY--;
                break;
        }
        return diggerCommand;
    }

    public bool DeadInConflict(ICreature conflictedObject) => false;

    public int GetDrawingPriority() => 0;

    public string GetImageFileName() => "Digger.png";
}

class Terrain : ICreature
{
    public CreatureCommand Act(int x, int y) => new();

    public bool DeadInConflict(ICreature conflictedObject) => true;

    public int GetDrawingPriority() => 1;

    public string GetImageFileName() => "Terrain.png";
}