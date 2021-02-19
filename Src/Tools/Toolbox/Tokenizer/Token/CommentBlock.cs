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
    public struct CommentBlock : IToken, IEquatable<CommentBlock>
    {
        public CommentBlock(string value, TextSpan textSpan)
        {
            Value = value;
            TextSpan = textSpan;
        }

        public TextSpan TextSpan { get; }

        public TokenType TokenType => TokenType.Data;

        public string Value { get; }

        public bool IsQuoted => false;

        public override string ToString() => $"TextSpan={TextSpan}, TokenType={TokenType}, Value=\"{Value}\"";

        public override bool Equals(object? obj) => obj is CommentBlock token && Equals(token);

        public bool Equals(CommentBlock other) =>
            TextSpan.Equals(other.TextSpan) &&
            TokenType == other.TokenType &&
            Value == other.Value;

        public override int GetHashCode() => HashCode.Combine(TextSpan, TokenType, Value);

        public static bool operator ==(CommentBlock left, CommentBlock right) => left.Equals(right);

        public static bool operator !=(CommentBlock left, CommentBlock right) => !(left == right);
    }
}
