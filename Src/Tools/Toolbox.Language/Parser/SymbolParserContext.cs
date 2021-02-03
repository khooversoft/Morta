using Microsoft.Extensions.DependencyInjection;
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
        public SymbolParserContext(IEnumerable<IToken> tokens, Action<string> logger)
        {
            tokens.VerifyNotNull(nameof(tokens));
            logger.VerifyNotNull(nameof(logger));

            InputTokens = new CursorList<IToken>(tokens);
            Logger = logger;
        }

        public CursorList<IToken> InputTokens { get; }

        public Action<string> Logger { get; }
    }
}
