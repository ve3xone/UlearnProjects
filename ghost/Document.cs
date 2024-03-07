using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hashes;

public class Document
{
	private readonly Encoding encoding;
	private readonly byte[] content;
	public string Title { get; }
	public string Content => encoding.GetString(content);


	public Document(string title, Encoding encoding, byte[] content)
	{
		Title = title;
		this.encoding = encoding;
		this.content = content;
	}

	protected bool Equals(Document other)
	{
		return string.Equals(Title, other.Title)
		       && Content.Equals(other.Content);
	}

	public override bool Equals(object obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != GetType()) return false;
		return Equals((Document) obj);
	}

	public override int GetHashCode()
	{
		unchecked
		{
			return Title.GetHashCode() * 397 ^ Content.GetHashCode();
		}
	}
}