using Avalonia.Input;
using Digger.Architecture;

namespace Digger;

class Player : ICreature
{
    public CreatureCommand Act(int x, int y)
    {
        CreatureCommand diggerCommand = new(){
            DeltaX = 0,
            DeltaY = 0
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

    public bool DeadInConflict(ICreature conflictedObject) => conflictedObject is Sack;

    public int GetDrawingPriority() => 0;

    public string GetImageFileName() => "Digger.png";
}

class Terrain : ICreature
{
	public CreatureCommand Act(int x, int y) => new()
	{
            DeltaX = 0,
            DeltaY = 0
    };

    public bool DeadInConflict(ICreature conflictedObject) => true;

    public int GetDrawingPriority() => 5;

    public string GetImageFileName() => "Terrain.png";
}

class Sack : ICreature
{
    public int CountFall = 0;
    public bool SackFalling = false;
    public CreatureCommand Act(int x, int y)
    {
        if (y + 1 < Game.MapHeight &&
            (Game.Map[x, y + 1] == null ||
            (Game.Map[x, y + 1] is Player &&
            SackFalling)))
        {
            CountFall++;
            SackFalling = true;
            return new CreatureCommand()
            {
                DeltaX = 0,
                DeltaY = 1
            };
		}
        if (CountFall > 1)
            return new CreatureCommand()
            {
                DeltaX = 0,
                DeltaY = 0,
                TransformTo = new Gold()
			};
        CountFall = 0;
		SackFalling = false;
        return new CreatureCommand()
        {
            DeltaX = 0,
            DeltaY = 0
        };
    }

    public bool DeadInConflict(ICreature conflictedObject) => false;

    public int GetDrawingPriority() => 3;

    public string GetImageFileName() => "Sack.png";
}

class Gold : ICreature
{
    public CreatureCommand Act(int x, int y) => new();

    public bool DeadInConflict(ICreature conflictedObject)
    {
        if (conflictedObject is Player)
            Game.Scores += 10;
        return true;
    }

    public int GetDrawingPriority() => 3;

    public string GetImageFileName() => "Gold.png";
}