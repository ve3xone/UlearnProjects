using System;

namespace hashes;

public class Animal
{
	public Animal(string name, DateTime birthDate)
	{
		BirthDate = birthDate;
		Name = name;
	}

	public string Name { get; private set; }
	public DateTime BirthDate { get; private set; }

	public override string ToString()
	{
		return $"{nameof(Name)}: {Name}, {nameof(BirthDate)}: {BirthDate}";
	}

	public void Rename(string newName)
	{
		Name = newName;
	}

}