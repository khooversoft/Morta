using System;
using System.Collections.Generic;
using System.Text;

namespace Toolbox.Tokenizer.Token
{
    /// <summary>
    /// Token value extracted from data
    /// </summary>
    public struct TokenValue : IToken, IEquatable<TokenValue>
    {
        public TokenValue(string value, TokenType tokenType, TextSpan textSpan)
        {
            TokenType = tokenType;
            TextSpan = textSpan;
            Value = value;
        }

        public TokenType TokenType { get; }

        public TextSpan TextSpan { get; }

        public string Value { get; }

        public bool IsQuoted => false;

        public override string ToString() => $"TokenType={TokenType}, TextSpan={TextSpan}, Value=\"{Value}\"";

        public override bool Equals(object? obj) => obj is TokenValue value && Equals(value);

        public bool Equals(TokenValue other) =>
            TokenType == other.TokenType &&
            TextSpan.Equals(other.TextSpan) &&
            Value == other.Value;

        public override int GetHashCode() => HashCode.Combine(TokenType, TextSpan, Value);

        public static bool operator ==(TokenValue left, TokenValue right) => left.Equals(right);

        public static bool operator !=(TokenValue left, TokenValue right) => !(left == right);
    }
}
