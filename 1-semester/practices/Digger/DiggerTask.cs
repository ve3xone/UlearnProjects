using Avalonia.Input;
using Digger.Architecture;

namespace Digger;

public class Terrain : ICreature
{
    public string GetImageFileName() => "Terrain.png";

    public int GetDrawingPriority() => 100;

    public CreatureCommand Act(int x, int y)=> new()
    {
        DeltaX = 0,
        DeltaY = 0,
        TransformTo = this
    };

    public bool DeadInConflict(ICreature conflictedObject) => conflictedObject is Player;
}

public class Player : ICreature
{
    public string GetImageFileName() => "Digger.png";

    public int GetDrawingPriority() => 0;

    public CreatureCommand Act(int x, int y)
    {
        CreatureCommand diggerCommand = new()
        {
            DeltaX = 0,
            DeltaY = 0,
            TransformTo = this
        };
        switch (Game.KeyPressed)
        {
            case Key.Up:
                if (IsMoveValid(x, y - 1))
                    diggerCommand.DeltaY--;
                break;

            case Key.Down:
                if (IsMoveValid(x, y + 1))
                    diggerCommand.DeltaY++;
                break;

            case Key.Right:
                if (IsMoveValid(x + 1, y))
                    diggerCommand.DeltaX++;
                break;

            case Key.Left:
                if (IsMoveValid(x - 1, y))
                    diggerCommand.DeltaX--;
                break;
        }
        return diggerCommand;
    }

    private static bool IsMoveValid(int targetX, int targetY)
    {
        return targetX >= 0 &&
               targetX < Game.MapWidth &&
               targetY >= 0 &&
               targetY < Game.MapHeight &&
               Game.Map[targetX, targetY] is not Sack;
    }

    public static bool CanGoTo(int x, int y)
    {
        if (x < 0 || y < 0 || 
            x >= Game.MapWidth || 
            y >= Game.MapHeight)
            return false;
        return Game.Map.GetValue(x, y) is not Sack;
    }

    public bool DeadInConflict(ICreature conflictedObject) 
        => conflictedObject is Sack || conflictedObject is Monster;
}

public class Sack : ICreature
{
    public int FlightTime;

    public string GetImageFileName() => "Sack.png";

    public int GetDrawingPriority() => 10;

    public CreatureCommand Act(int x, int y)
    {
        var command = new CreatureCommand
        {
            DeltaX = 0,
            DeltaY = 1,
            TransformTo = this
        };

        if (CanFallTo(x + command.DeltaX, y + command.DeltaY))
        {
            FlightTime++;
        }
        else
        {
            if (FlightTime > 1)
                command.TransformTo = new Gold();
            FlightTime = 0;
            command.DeltaY = 0;
        }

        return command;
    }

    public bool CanFallTo(int x, int y)
    {
        if (x < 0 ||
            y < 0 ||
            x >= Game.MapWidth ||
            y >= Game.MapHeight)
            return false;
        var cell = Game.Map.GetValue(x, y);
        return cell == null ||
            (cell is Monster ||
            cell is Player) &&
               FlightTime > 0;
    }

    public bool DeadInConflict(ICreature conflictedObject) => false;
}

public class Gold : ICreature
{
    public string GetImageFileName() => "Gold.png";

    public int GetDrawingPriority() => 10;

    public CreatureCommand Act(int x, int y) => new()
    {
        DeltaX = 0,
        DeltaY = 0,
        TransformTo = this
    };

    public bool DeadInConflict(ICreature conflictedObject)
    {
        if (conflictedObject is Player)
            Game.Scores += 10;
        return true;
    }
}

public class Monster : ICreature
{
    public string GetImageFileName() => "Monster.png";

    public int GetDrawingPriority() => 20;

    public CreatureCommand Act(int x, int y)
    {
        var diggerCommand = new CreatureCommand
        {
            DeltaX = 0,
            DeltaY = 0,
            TransformTo = this
        };

        if (IsPlayerInSection(0, 0, x, Game.MapHeight) &&
            CanGoTo(x - 1, y))
            diggerCommand.DeltaX = -1;
        else if (IsPlayerInSection(x + 1, 0, Game.MapWidth, Game.MapHeight) &&
                 CanGoTo(x + 1, y))
            diggerCommand.DeltaX = 1;
        else if (IsPlayerInSection(0, 0, Game.MapWidth, y) && 
                 CanGoTo(x, y - 1))
            diggerCommand.DeltaY = -1;
        else if (IsPlayerInSection(0, y + 1, Game.MapWidth, Game.MapHeight) && 
                 CanGoTo(x, y + 1))
            diggerCommand.DeltaY = 1;

        return diggerCommand;
    }

    private bool IsPlayerInSection(int x0, int y0,
        int x1, int y1)
    {
        for (var x = x0; x < x1; x++)
            for (var y = y0; y < y1; y++)
                if (Game.Map.GetValue(x, y) is Player)
                    return true;
        return false;
    }

    private bool CanGoTo(int x, int y)
    {
        if (x < 0 ||
            y < 0 ||
            x >= Game.MapWidth ||
            y >= Game.MapHeight)
            return false;
        var cell = Game.Map.GetValue(x, y);
        return cell == null ||
               !(cell is Sack ||
               cell is Monster ||
               cell is Terrain);
    }

    public bool DeadInConflict(ICreature conflictedObject)
        => conflictedObject is Monster ||
        conflictedObject is Sack &&
        ((Sack)conflictedObject).FlightTime > 0;
}