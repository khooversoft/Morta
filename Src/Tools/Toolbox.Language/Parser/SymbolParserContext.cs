using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Tokenizer.Token;
using Toolbox.Tools;

namespace Toolbox.Language.Parser
{
    public class SymbolParserContext
    {
        public SymbolParserContext(IEnumerable<IToken> tokens)
        {
            tokens.VerifyNotNull(nameof(tokens));

            InputTokens = new CursorList<IToken>(tokens);
        }

        public CursorList<IToken> InputTokens { get; }

        public List<List<ISymbolToken>> DebugStack { get; } = new List<List<ISymbolToken>>();

        public Stack<string> LocationStack { get; } = new Stack<string>();
    }
}
