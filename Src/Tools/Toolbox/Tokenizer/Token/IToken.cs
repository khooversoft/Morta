using System;
using System.Collections.Generic;
using System.Text;

namespace Toolbox.Tokenizer.Token
{
    /// <summary>
    ///  Standard token interface
    /// </summary>
    public interface IToken
    {
        TokenType TokenType { get; }

        string Value { get; }

        bool IsQuoted { get; }

        TextSpan TextSpan { get; }
    }
}