// ReSharper disable NonReadonlyMemberInGetHashCode
using System;

namespace hashes;

public class Cat : Animal
{
	private readonly string breed;

	public Cat(string name, string breed, DateTime birthDate) : base(name, birthDate)
	{
		this.breed = breed;
	}

	protected bool Equals(Cat other)
	{
		return string.Equals(breed, other.breed) 
		       && Name.Equals(other.Name) 
		       && BirthDate.Equals(other.BirthDate);
	}

	public override bool Equals(object obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((Cat) obj);
	}

	public override int GetHashCode()
	{
		return unchecked((breed.GetHashCode()*397 ^ Name.GetHashCode())*397 ^ BirthDate.GetHashCode());
	}
}