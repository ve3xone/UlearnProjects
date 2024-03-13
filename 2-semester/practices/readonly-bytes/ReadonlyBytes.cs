using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace hashes;

public class ReadonlyBytes : IEnumerable<byte>
{
    private readonly byte[] bytes;
    private readonly int hashCode;

    public ReadonlyBytes(params byte[] bytes)
    {
        this.bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
        unchecked
        {
            var hash = 2;
            for (var index = 0; index < this.bytes.Length; index++)
            {
                var bindex = this.bytes[index];
                hash *= 1010;
                hash += bindex;
            }
            hashCode = hash;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var other = (ReadonlyBytes)obj;

        if (Length != other.Length)
            return false;

        return bytes.SequenceEqual(other.bytes);
    }

    public int Length => bytes.Length;

    public byte this[int index] => bytes[index];

    public override int GetHashCode() => hashCode;

    public override string ToString() => $"[{string.Join(", ", bytes)}]";

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<byte> GetEnumerator() => ((IEnumerable<byte>)bytes).GetEnumerator();
}