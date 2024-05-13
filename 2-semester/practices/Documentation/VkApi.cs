using System;

namespace Documentation;

[ApiDescription("Vk API client")]
public class VkApi
{
	[ApiMethod]
	[ApiDescription("private authorize user. For internal use only")]
	private bool AuthorizePrivate()
	{
		throw new NotImplementedException();
	}

	// No [ApiMethod]
	[ApiDescription("Authorize user. Returns true if authorized")]
	public bool Authorize2([ApiRequired] string login, [ApiRequired] string password)
	{
		throw new NotImplementedException();
	}

	public bool EnterBackdoor([ApiRequired] string login, [ApiRequired] string password)
	{
		throw new NotImplementedException();
	}

	[ApiMethod]
	[ApiDescription("Authorize user. Returns true if authorized")]
	public void Authorize([ApiRequired] string login, [ApiRequired] string password, bool allowNoname = false)
	{
		throw new NotImplementedException();
	}

	[ApiMethod]
	[ApiDescription("Gets user audio tracks. If userId is not presented gets authorized user audio tracks")]
	[return: ApiRequired(false)]
	public VkAudio[] SelectAudio(
		[ApiRequired(false)] string userId,
		[ApiIntValidation(1, 100), ApiDescription("number of audios to return"), ApiRequired] int batchSize)
	{
		throw new NotImplementedException();
	}

	[ApiMethod]
	[ApiDescription("Gets user audio tracks count. If userId is not presented gets authorized user audio tracks")]
	[return: ApiRequired]
	[return: ApiIntValidation(0, int.MaxValue / 2)]
	public int GetTotalAudioCount([ApiRequired(false)] string userId)
	{
		throw new NotImplementedException();
	}
}

[ApiDescription("VkAudio information")]
public class VkAudio
{
	[ApiDescription("Track title")] public string Title { get; set; }

	[ApiDescription("Audio artists, separated by ',' ")]
	public string[] Artists { get; set; }
}