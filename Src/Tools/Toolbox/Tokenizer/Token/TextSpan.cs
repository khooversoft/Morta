using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Tools;

namespace Toolbox.Tokenizer.Token
{
    public struct TextSpan : IEquatable<TextSpan>
    {
        public TextSpan(int start, int length)
        {
            start.VerifyAssert(x => x >= 0, nameof(start));
            (start + length).VerifyAssert(x => x >= 0, nameof(length));

            Start = start;
            Length = length;
        }

        /// <summary>
        /// Start point of the span.
        /// </summary>
        public int Start { get; }

        /// <summary>
        /// Length of the span.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// End of the span.
        /// </summary>
        public int End => Start + Length;

        /// <summary>
        /// Determines whether or not the span is empty.
        /// </summary>
        public bool IsEmpty => this.Length == 0;

        public override string ToString() => $"Start={Start}, Length={Length}";

        public override bool Equals(object? obj) => obj is TextSpan span && Equals(span);

        public bool Equals(TextSpan other) => Start == other.Start && Length == other.Length;

        public override int GetHashCode() => HashCode.Combine(Start, Length);

        public static bool operator ==(TextSpan left, TextSpan right) => left.Equals(right);

        public static bool operator !=(TextSpan left, TextSpan right) => !(left == right);
    }
}
