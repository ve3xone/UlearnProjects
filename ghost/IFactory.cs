namespace hashes;

public interface IMagic
{
	void DoMagic();
}
public interface IFactory<out T>
{
	T Create();
}