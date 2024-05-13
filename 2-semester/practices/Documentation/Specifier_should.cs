using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Documentation;

[TestFixture]
public class Specifier_should
{
	private ISpecifier vkApiSpecifier;
	private ISpecifier notApiSpecifier;

	[SetUp]
	protected virtual void Setup()
	{
		vkApiSpecifier = new Specifier<VkApi>();
		notApiSpecifier = new Specifier<List<int>>();
	}

	private const string authorizeMethodName = "Authorize";
	private const string enterBackdoorMethodName = "EnterBackdoor";
	private const string selectAudioMethodName = "SelectAudio";
	private const string countAudioMethodName = "GetTotalAudioCount";

	[Test]
	public void GetApiDescription()
	{
		var description = vkApiSpecifier.GetApiDescription();
		Assert.AreEqual("Vk API client", description);
	}

	[Test]
	public void GetApiDescriptionWhenNotApi()
	{
		var description = notApiSpecifier.GetApiDescription();
		Assert.IsNull(description);
	}

	[Test]
	public void GetApiMethodNamesWithDescription()
	{
		var actual = vkApiSpecifier.GetApiMethodNames();
		CollectionAssert.AreEquivalent(
			new[] {authorizeMethodName, countAudioMethodName, selectAudioMethodName },
			actual);
	}

	[Test]
	public void GetApiMethodDescription()
	{
		var description = vkApiSpecifier.GetApiMethodDescription(authorizeMethodName);
		Assert.AreEqual("Authorize user. Returns true if authorized", description);
	}

	[Test]
	public void GetApiMethodDescriptionWhenRandomMethodName()
	{
		var description = vkApiSpecifier.GetApiMethodDescription(Guid.NewGuid().ToString());
		Assert.IsNull(description);
	}

	[Test]
	public void GetApiMethodDescriptionWithoutDescription()
	{
		var description = vkApiSpecifier.GetApiMethodDescription(enterBackdoorMethodName);
		Assert.IsNull(description);
	}

	[Test]
	public void GetApiMethodParamNames()
	{
		var description = vkApiSpecifier.GetApiMethodParamNames(authorizeMethodName);
		CollectionAssert.AreEquivalent(new[] {"login", "password", "allowNoname"}, description);
	}

	[Test]
	public void GetApiMethodParamNamesWhenRandomMethodName()
	{
		var description = vkApiSpecifier.GetApiMethodDescription(Guid.NewGuid().ToString());
		Assert.IsNull(description);
	}

	[Test]
	public void GetApiMethodParamDescription()
	{
		var description = vkApiSpecifier.GetApiMethodParamDescription(selectAudioMethodName, "batchSize");
		Assert.AreEqual("number of audios to return", description);
	}

	[Test]
	public void GetApiMethodParamDescriptionWhenRandomMethodName()
	{
		var description = vkApiSpecifier.GetApiMethodParamDescription(Guid.NewGuid().ToString(), "some");
		Assert.IsNull(description);
	}

	[Test]
	public void GetApiMethodParamDescriptionWhenRandomParamName()
	{
		var description = vkApiSpecifier.GetApiMethodParamDescription(selectAudioMethodName, Guid.NewGuid().ToString());
		Assert.IsNull(description);
	}

	[Test]
	public void GetApiMethodParamDescriptionWithoutDescription()
	{
		var description = vkApiSpecifier.GetApiMethodParamDescription(authorizeMethodName, "allowNoname");
		Assert.IsNull(description);
	}

	[Test]
	public void GetApiMethodParamFullDescription()
	{
		var description = vkApiSpecifier.GetApiMethodParamFullDescription(selectAudioMethodName, "batchSize");
		Assert.IsNotNull(description);
		Assert.AreEqual(1, description.MinValue);
		Assert.AreEqual(100, description.MaxValue);
		Assert.IsTrue(description.Required);
		var expected = new CommonDescription("batchSize", "number of audios to return");
		AssertCommonDescriptionAreEquals(expected, description.ParamDescription);
	}

	[Test]
	public void GetApiMethodParamFullDescriptionWhenRandomMethodName()
	{
		var paramName = "some";
		var description = vkApiSpecifier.GetApiMethodParamFullDescription(Guid.NewGuid().ToString(), paramName);
		Assert.IsNotNull(description);
		Assert.IsNull(description.MinValue);
		Assert.IsNull(description.MaxValue);
		Assert.IsFalse(description.Required);
		var expected = new CommonDescription(paramName);
		AssertCommonDescriptionAreEquals(expected, description.ParamDescription);
	}

	[Test]
	public void GetApiMethodParamFullDescriptionWhenRandomParamName()
	{
		var paramName = Guid.NewGuid().ToString();
		var description = vkApiSpecifier.GetApiMethodParamFullDescription(authorizeMethodName, paramName);
		Assert.IsNotNull(description);
		Assert.IsNull(description.MinValue);
		Assert.IsNull(description.MaxValue);
		Assert.IsFalse(description.Required);
		var expected = new CommonDescription(paramName);
		AssertCommonDescriptionAreEquals(expected, description.ParamDescription);
	}

	[Test]
	public void GetApiMethodParamFullDescriptionNotAllAttributes()
	{
		var description = vkApiSpecifier.GetApiMethodParamFullDescription(authorizeMethodName, "login");
		Assert.IsNotNull(description);
		Assert.IsNull(description.MinValue);
		Assert.IsNull(description.MaxValue);
		Assert.IsTrue(description.Required);
		var expected = new CommonDescription("login");
		AssertCommonDescriptionAreEquals(expected, description.ParamDescription);
	}

	[Test]
	public void GetApiMethodFullDescriptionReturnsNullWhenApiMethodAttributeMissing()
	{
		var description = vkApiSpecifier.GetApiMethodFullDescription("Authorize2");
		Assert.IsNull(description);
	}

	[Test]
	public void GetApiMethodFullDescriptionAuthorize()
	{
		var description = vkApiSpecifier.GetApiMethodFullDescription(authorizeMethodName);
		Assert.IsNotNull(description);
		var expected = new ApiMethodDescription
		{
			MethodDescription = new CommonDescription(authorizeMethodName,
				"Authorize user. Returns true if authorized"),
			ParamDescriptions = new[]
			{
				new ApiParamDescription
				{
					ParamDescription = new CommonDescription("login"),
					Required = true
				},
				new ApiParamDescription
				{
					ParamDescription = new CommonDescription("password"),
					Required = true
				},
				new ApiParamDescription
				{
					ParamDescription = new CommonDescription("allowNoname"),
				},
			}
		};
		AssertDescriptionAreEquals(expected, description);
	}

	[Test]
	public void GetApiMethodFullDescriptionSelectAudio()
	{
		var description = vkApiSpecifier.GetApiMethodFullDescription(selectAudioMethodName);
		Assert.IsNotNull(description);
		var expected = new ApiMethodDescription
		{
			MethodDescription = new CommonDescription(selectAudioMethodName,
				"Gets user audio tracks. If userId is not presented gets authorized user audio tracks"),
			ParamDescriptions = new[]
			{
				new ApiParamDescription
				{
					ParamDescription = new CommonDescription("userId"),
				},
				new ApiParamDescription
				{
					ParamDescription = new CommonDescription("batchSize", "number of audios to return"),
					Required = true,
					MinValue = 1,
					MaxValue = 100
				},
			},
			ReturnDescription = new ApiParamDescription
			{
				ParamDescription = new CommonDescription()
			}
		};
		AssertDescriptionAreEquals(expected, description);
	}

	[Test]
	public void GetApiMethodFullDescriptionCountAudio()
	{
		var description = vkApiSpecifier.GetApiMethodFullDescription(countAudioMethodName);
		Assert.IsNotNull(description);
		var expected = new ApiMethodDescription
		{
			MethodDescription = new CommonDescription(countAudioMethodName,
				"Gets user audio tracks count. If userId is not presented gets authorized user audio tracks"),
			ParamDescriptions = new[]
			{
				new ApiParamDescription
				{
					ParamDescription = new CommonDescription("userId"),
				},
			},
			ReturnDescription = new ApiParamDescription
			{
				Required = true,
				ParamDescription = new CommonDescription(),
				MinValue = 0,
				MaxValue = int.MaxValue / 2
			}
		};
		AssertDescriptionAreEquals(expected, description);
	}

	#region AssertHelpers

	private static void AssertDescriptionAreEquals(ApiMethodDescription expected, ApiMethodDescription actual)
	{
		AssertCommonDescriptionAreEquals(expected.MethodDescription, actual.MethodDescription);

		AssertParamDescriptionAreEquals(expected.ReturnDescription, actual.ReturnDescription);

		if (expected.ParamDescriptions == null && actual.ParamDescriptions == null)
		{
			return;
		}

		Assert.AreEqual(expected.ParamDescriptions?.Length, actual.ParamDescriptions?.Length);

		var expectedParamDescriptions = expected.ParamDescriptions.OrderBy(x => x.ParamDescription?.Name).ToArray();
		var actualParamDescriptions = actual.ParamDescriptions.OrderBy(x => x.ParamDescription?.Name).ToArray();

		for (var i = 0; i < expected.ParamDescriptions.Length; i++)
		{
			AssertParamDescriptionAreEquals(expectedParamDescriptions[i], actualParamDescriptions[i]);
		}
	}

	private static void AssertParamDescriptionAreEquals(ApiParamDescription expected, ApiParamDescription actual)
	{
		Assert.AreEqual(expected?.MaxValue, actual?.MaxValue);
		Assert.AreEqual(expected?.MinValue, actual?.MinValue);
		Assert.AreEqual(expected?.Required, actual?.Required);

		AssertCommonDescriptionAreEquals(expected?.ParamDescription, actual?.ParamDescription);
	}

	private static void AssertCommonDescriptionAreEquals(CommonDescription expected, CommonDescription actual)
	{
		Assert.AreEqual(expected?.Name, actual?.Name);
		Assert.AreEqual(expected?.Description, actual?.Description);
	}

	#endregion
}