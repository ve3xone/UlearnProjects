namespace Documentation;

public interface ISpecifier
{
	string GetApiDescription();
	string[] GetApiMethodNames();
	string GetApiMethodDescription(string methodName);
	string[] GetApiMethodParamNames(string methodName);
	string GetApiMethodParamDescription(string methodName, string paramName);
	ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName);
	ApiMethodDescription GetApiMethodFullDescription(string methodName);
}

public class CommonDescription
{
	public CommonDescription()
	{
	}

	public CommonDescription(string name, string description = null)
	{
		Name = name;
		Description = description;
	}

	public string Name { get; set; }
	public string Description { get; set; }
}

public class ApiMethodDescription
{
	public CommonDescription MethodDescription { get; set; }
	public ApiParamDescription[] ParamDescriptions { get; set; }
	public ApiParamDescription ReturnDescription { get; set; }
}

public class ApiParamDescription
{
	public CommonDescription ParamDescription { get; set; }

	public bool Required { get; set; }
	public object MinValue { get; set; }
	public object MaxValue { get; set; }
}

public class ApiClassDescription
{
	public ApiMethodDescription[] MethodDescriptions { get; set; }
}