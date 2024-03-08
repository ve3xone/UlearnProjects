namespace LimitedSizeStack;

public class Counter
{
	public int Value { get; private set; }

	public Counter()
	{
		Value = 0;
	}

	public void Increase()
	{
		Value++;
	}
}

// Класс, который увеличивает значение Counter-а каждый раз, когда сборщик мусора собирает объект этого класса.
// Нужен, чтобы протестировать, что стек не оставляет указателей на вытесненные из стека объекты.
class FinalizableClass
{
	public Counter Counter;

	public FinalizableClass(Counter counter)
	{
		Counter = counter;
	}

	// Это деструктор. Специальный метод, который вызывается сборщиком мусора, перед тем как освободить память от этого объекта.
	~FinalizableClass()
	{
		lock (Counter)
		{
			Counter.Increase();
		}
	}
}