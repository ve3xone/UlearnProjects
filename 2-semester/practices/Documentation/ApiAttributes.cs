using System;

namespace Documentation;

[AttributeUsage(AttributeTargets.Method)]
public class ApiMethodAttribute : Attribute
{
}

public class ApiDescriptionAttribute : Attribute
{
	public ApiDescriptionAttribute(string description)
	{
		Description = description;
	}

	public string Description { get; }
}

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
public class ApiIntValidationAttribute : Attribute
{
	public ApiIntValidationAttribute(int minValue, int maxValue)
	{
		MaxValue = maxValue;
		MinValue = minValue;
	}

	public int? MaxValue { get; }
	public int? MinValue { get; }
}

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
public class ApiRequiredAttribute : Attribute
{
	public ApiRequiredAttribute(bool required = true)
	{
		Required = required;
	}

	public bool Required { get; }
}