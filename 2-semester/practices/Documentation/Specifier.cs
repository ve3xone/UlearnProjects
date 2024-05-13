using System;
using System.Linq;
using System.Reflection;

namespace Documentation;

public class Specifier<T> : ISpecifier
{
	public string GetApiDescription()
	{
		throw new NotImplementedException();
	}

	public string[] GetApiMethodNames()
	{
		throw new NotImplementedException();
	}

	public string GetApiMethodDescription(string methodName)
	{
		throw new NotImplementedException();
	}

	public string[] GetApiMethodParamNames(string methodName)
	{
		throw new NotImplementedException();
	}

	public string GetApiMethodParamDescription(string methodName, string paramName)
	{
		throw new NotImplementedException();
	}

	public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName)
	{
		throw new NotImplementedException();
	}

	public ApiMethodDescription GetApiMethodFullDescription(string methodName)
	{
		throw new NotImplementedException();
	}
}