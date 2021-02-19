using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Toolbox.Tokenizer.Token
{
    /// <summary>
    /// Block token that has been extracted from the data.
    /// </summary>
    /// 
    public struct BlockToken : IToken, IEquatable<BlockToken>
    {
        public BlockToken(string value, TextSpan textSpan)
        {
            BlockSignal = value[0];

            if (value.Length < 2) throw new ArgumentException("Length to small for quoted data");
            if (value[value.Length - 1] != BlockSignal) throw new ArgumentException("Ending quote does not match beginning");

            Value = value.Substring(1, value.Length - 2);
            TextSpan = textSpan;
        }

        public char BlockSignal { get; }

        public TextSpan TextSpan { get; }

        public TokenType TokenType => TokenType.Data;

        public string Value { get; }

        public bool IsQuoted => true;

        public override string ToString() => $"BlockSignal={BlockSignal}, TextSpan={TextSpan}, TokenType={TokenType}, Value=\"{Value}\"";

        public override bool Equals(object? obj) => obj is BlockToken token && Equals(token);

        public bool Equals(BlockToken other) =>
            BlockSignal == other.BlockSignal &&
            TextSpan.Equals(other.TextSpan) &&
            TokenType == other.TokenType &&
            Value == other.Value;

        public override int GetHashCode() => HashCode.Combine(BlockSignal, TextSpan, TokenType, Value);

        public static bool operator ==(BlockToken left, BlockToken right) => left.Equals(right);

        public static bool operator !=(BlockToken left, BlockToken right) => !(left == right);
    }
}
