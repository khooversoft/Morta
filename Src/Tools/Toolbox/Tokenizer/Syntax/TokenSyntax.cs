﻿using System;
using Toolbox.Tokenizer.Token;

namespace Toolbox.Tokenizer.Syntax
{
    /// <summary>
    /// Provides the token syntax definition for a general string token.
    /// </summary>
    public record TokenSyntax : ITokenSyntax
    {
        public TokenSyntax(string token, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            Token = token;
            StringComparison = stringComparison;
            Priority = 3 + Token.Length;
        }

        public string Token { get; }

        public StringComparison StringComparison { get; }

        public int Priority { get; }

        public int? Match(ReadOnlySpan<char> span)
        {
            if (Token.Length > span.Length) return null;

            ReadOnlySpan<char> slice = span.Slice(0, Token.Length);
            if (Token.AsSpan().CompareTo(slice, StringComparison) == 0) return Token.Length;

            return null;
        }

        public IToken CreateToken(ReadOnlySpan<char> span, TextSpan textSpan)
        {
            string value = span.ToString();
            return new TokenValue(value, TokenType.ParseToken, textSpan);
        }
    }
}