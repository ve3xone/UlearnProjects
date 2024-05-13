using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Documentation;

public class Specifier<T> : ISpecifier
{
    private readonly Type _type;

    public Specifier()
    {
        _type = typeof(T);
    }

    public string GetApiDescription()
    {
        return _type.GetCustomAttribute<ApiDescriptionAttribute>()?.Description;
    }

    public string[] GetApiMethodNames()
    {
        return _type.GetMethods()
                    .Where(method => method.IsPublic && method.GetCustomAttributes(true)
                                                                .OfType<ApiMethodAttribute>().Any())
                                                                .Select(method => method.Name)
                                                                .ToArray();
    }

    private MethodInfo GetMethodByName(string methodName)
    {
        return _type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
    }

    private ParameterInfo GetParameterByName(string methodName, string paramName)
    {
        var method = GetMethodByName(methodName);
        return method?.GetParameters().FirstOrDefault(param => param.Name == paramName);
    }

    public string GetApiMethodDescription(string methodName)
    {
        var method = GetMethodByName(methodName);
        return method?.GetCustomAttribute<ApiDescriptionAttribute>()?.Description;
    }

    public string[] GetApiMethodParamNames(string methodName)
    {
        var method = GetMethodByName(methodName);
        return method?.GetParameters().Select(param => param.Name).ToArray() ?? Array.Empty<string>();
    }

    public string GetApiMethodParamDescription(string methodName, string paramName)
    {
        var parameter = GetParameterByName(methodName, paramName);
        return parameter?.GetCustomAttribute<ApiDescriptionAttribute>()?.Description;
    }

    public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName)
    {
        var result = new ApiParamDescription();
        result.ParamDescription = new CommonDescription(paramName);
        var met = _type.GetMethod(methodName);
        if (met != null && met.GetCustomAttributes(true).OfType<ApiMethodAttribute>().Any())
        {
            var parameter = met.GetParameters().Where(param => param.Name == paramName);
            if (parameter.Any())
            {
                var descriptionAttribute = GetApiDescriptionAttribute(parameter);
                if (descriptionAttribute != null)
                    result.ParamDescription.Description = descriptionAttribute.Description;
                var intValidationAttribute = GetApiIntValidationAttribute(parameter);
                if (intValidationAttribute != null)
                    SetIntValidationAttributes(result, intValidationAttribute);
                var requiredAttribute = GetApiRequiredAttribute(parameter);
                if (requiredAttribute != null)
                    result.Required = requiredAttribute.Required;
            }
        }
        return result;
    }

    private ApiDescriptionAttribute GetApiDescriptionAttribute(IEnumerable<ParameterInfo> parameter)
    {
        return parameter.First()
                        .GetCustomAttributes(true)
                        .OfType<ApiDescriptionAttribute>()
                        .FirstOrDefault();
    }

    private ApiIntValidationAttribute GetApiIntValidationAttribute(IEnumerable<ParameterInfo> parameter)
    {
        return parameter.First()
                        .GetCustomAttributes(true)
                        .OfType<ApiIntValidationAttribute>()
                        .FirstOrDefault();
    }

    private ApiRequiredAttribute GetApiRequiredAttribute(IEnumerable<ParameterInfo> parameter)
    {
        return parameter.First()
                        .GetCustomAttributes(true)
                        .OfType<ApiRequiredAttribute>()
                        .FirstOrDefault();
    }

    private void SetIntValidationAttributes(ApiParamDescription result,
                                            ApiIntValidationAttribute intValidationAttribute)
    {
        result.MinValue = intValidationAttribute.MinValue;
        result.MaxValue = intValidationAttribute.MaxValue;
    }

    public ApiMethodDescription GetApiMethodFullDescription(string methodName)
    {
        var met = _type.GetMethod(methodName);
        if (met == null || !met.GetCustomAttributes(true).OfType<ApiMethodAttribute>().Any())
            return null;
        var result = new ApiMethodDescription();
        result.MethodDescription = new CommonDescription(methodName, GetApiMethodDescription(methodName));
        result.ParamDescriptions = GetApiMethodParamNames(methodName).Select(param =>
                                   GetApiMethodParamFullDescription(methodName, param)).ToArray();
        SetReturnDescription(result, met.ReturnParameter);
        return result;
    }

    private void SetReturnDescription(ApiMethodDescription result, ParameterInfo returnParameter)
    {
        var returnParamDiscription = new ApiParamDescription();
        returnParamDiscription.ParamDescription = new CommonDescription();
        var descriptionAttribute = returnParameter.GetCustomAttributes(true)
                                                  .OfType<ApiDescriptionAttribute>()
                                                  .FirstOrDefault();
        if (descriptionAttribute != null)
            returnParamDiscription.ParamDescription.Description = descriptionAttribute.Description;
        var intValidationAttribute = returnParameter.GetCustomAttributes(true)
                                                    .OfType<ApiIntValidationAttribute>()
                                                    .FirstOrDefault();
        if (intValidationAttribute != null)
            SetIntValidationAttributes(returnParamDiscription, intValidationAttribute);
        var requiredAttribute = returnParameter.GetCustomAttributes(true)
                                               .OfType<ApiRequiredAttribute>()
                                               .FirstOrDefault();
        if (requiredAttribute != null)
            returnParamDiscription.Required = requiredAttribute.Required;
        if (descriptionAttribute != null || intValidationAttribute != null || requiredAttribute != null)
            result.ReturnDescription = returnParamDiscription;
    }
}