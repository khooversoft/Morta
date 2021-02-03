using System;
using System.Collections.Generic;
using System.Text;

namespace Toolbox.Tokenizer.Token
{
    /// <summary>
    /// Token value extracted from data
    /// </summary>
    public struct TokenValue : IToken
    {
        public TokenValue(string value, TokenType tokenType)
        {
            TokenType = tokenType;
            Value = value;
        }

        public TokenType TokenType { get; }

        public string Value { get; }

        public override bool Equals(object? obj) => obj is TokenValue value && Value == value.Value;

        public override int GetHashCode() => HashCode.Combine(Value);

        public override string ToString() => Value;

        public static bool operator !=(TokenValue left, TokenValue right) => !(left == right);

        public static bool operator ==(TokenValue left, TokenValue right) => left.Equals(right);
    }
}
