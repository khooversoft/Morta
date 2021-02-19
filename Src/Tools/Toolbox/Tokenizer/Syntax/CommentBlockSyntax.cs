using System;
using System.Collections.Generic;
using System.Text;
using Toolbox.Tokenizer.Token;

namespace Toolbox.Tokenizer.Syntax
{
    /// <summary>
    /// Handles comment data, starts with "//" and ends at the end of the line
    /// </summary>
    public class CommentBlockSyntax : ITokenSyntax
    {
        public int Priority => 1;

        public int? Match(ReadOnlySpan<char> span)
        {
            if (span.Length < 2) return null;
            if (span[0] != '/' || span[1] != '/') return null;

            for (int index = 2; index < span.Length; index++)
            {
                if (span[index] == "\n"[0]) return index + 1;
            }

            return null;
        }

        public IToken CreateToken(ReadOnlySpan<char> span, TextSpan textSpan)
        {
            string value = span.ToString();
            if (value.EndsWith("\n")) value = value[..^1];

            return new CommentBlock(value, textSpan);
        }
    }
}
