using System.Collections.Generic;

namespace Clones;

public class CloneVersionSystem : ICloneVersionSystem
{
    private readonly List<Clone> clones = new List<Clone> { new Clone() };

    public string Execute(string query)
    {
        var parts = query.Split(' ');
        var command = parts[0];
        var cloneNumber = int.Parse(parts[1]);
        var clone = clones[cloneNumber - 1];

        switch (command)
        {
            case "learn":
                clone.Learn(int.Parse(parts[2]));
                break;
            case "rollback":
                clone.Rollback();
                break;
            case "relearn":
                clone.Relearn();
                break;
            case "clone":
                clones.Add(new Clone(clone));
                break;
            case "check":
                return clone.Check();
        }
        return null;
    }
}

public class Clone
{
    public ListItem LearnHistory;
    public ListItem RollbackHistory;

    public Clone(){ }

    public Clone(Clone clone)
    {
        LearnHistory = clone.LearnHistory;
        RollbackHistory = clone.RollbackHistory;
    }

    public void Learn(int n)
    {
        LearnHistory = new ListItem(n, LearnHistory);
        RollbackHistory = null;
    }

    public void Rollback()
    {
        RollbackHistory = new ListItem(LearnHistory.Value, RollbackHistory);
        LearnHistory = LearnHistory.Prev;
    }

    public void Relearn()
    {
        if (RollbackHistory != null)
        {
            LearnHistory = new ListItem(RollbackHistory.Value, LearnHistory);
            RollbackHistory = RollbackHistory.Prev;
        }
    }

    public string Check()
    {
        return LearnHistory?.Value.ToString() ?? "basic";
    }
}

public class ListItem
{
    public int Value;
    public ListItem Prev;

    public ListItem(int value, ListItem prev)
    {
        Value = value;
        Prev = prev;
    }
}