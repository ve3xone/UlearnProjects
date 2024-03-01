using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace hashes
{
    public class ReadonlyBytes : IEnumerable<byte>
    {
        private readonly byte[] _bytes;

        public ReadonlyBytes(params byte[] bytes)
        {
            _bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
        }

        public int Length => _bytes.Length;

        public byte this[int index] => _bytes[index];

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (ReadonlyBytes)obj;

            if (Length != other.Length)
                return false;

            return _bytes.SequenceEqual(other._bytes);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                foreach (byte b in _bytes)
                {
                    hash = hash * 31 + b.GetHashCode();
                }
                return hash;
            }
        }

        public IEnumerator<byte> GetEnumerator()
        {
            foreach (var b in _bytes)
            {
                yield return b;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString()
        {
            return "[" + string.Join(", ", _bytes) + "]";
        }
    }
}
