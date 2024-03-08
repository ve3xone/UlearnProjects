using System;
using System.Text;

namespace hashes;

public class GhostsTask :
    IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>,
    IMagic
{
    readonly Cat cat = new("Абоба", "Сфинкс", new DateTime(2020, 8, 15));
    readonly Vector vector = new(10, 10);
    readonly Robot robot = new("11", 654.987);

    static readonly Encoding unicode = Encoding.Unicode;
    readonly byte[] content = unicode.GetBytes("Сомнительно, но окей.)");
    Document document;
    Segment segment;

    public Cat Create()
    {
        return cat;
    }

    public void DoMagic()
    {
        cat.Rename("Абобчанский");
        vector.Add(new Vector(5, 5));
        Robot.BatteryCapacity++;
        content[0] = 12;
    }

    Vector IFactory<Vector>.Create()
    {
        return vector;
    }

    Robot IFactory<Robot>.Create()
    {
        return robot;
    }

    Document IFactory<Document>.Create()
    {
        document = new("Заголовок", unicode, content);
        return document;
    }

    Segment IFactory<Segment>.Create()
    {
        segment = new(vector, new Vector(1, 1));
        return segment;
    }
}