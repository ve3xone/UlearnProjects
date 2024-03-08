namespace Clones;

public interface ICloneVersionSystem
{
	string Execute(string query);
}

public class Factory
{
	public static ICloneVersionSystem CreateCVS()
	{
		return new CloneVersionSystem();
		//return new checking.CloneVersionSystemSolved();
	}
}