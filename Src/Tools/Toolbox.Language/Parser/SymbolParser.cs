using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Extensions;
using Toolbox.Language.Grammar;
using Toolbox.Language.ProcessingRules;
using Toolbox.Tokenizer;
using Toolbox.Tokenizer.Token;
using Toolbox.Tools;

namespace Toolbox.Language.Parser
{
    public class SymbolParser<T> where T : Enum
    {
        private readonly CodeBlock<T> _productionRules;
        private readonly TokenParser<T> _tokenizer;

        public SymbolParser(CodeBlock<T> productionRules)
        {
            productionRules.VerifyNotNull(nameof(productionRules));

            _productionRules = productionRules;
            _tokenizer = new TokenParser<T>(_productionRules);
        }

        public SymbolParserResponse<T> Parse(params string[] rawLines)
        {
            string rawData = rawLines.Aggregate(string.Empty, (a, x) => a += x);

            IReadOnlyList<IToken> tokens = _tokenizer.Parse(rawData);
            if (tokens.Count == 0) return new SymbolParserResponse<T>();

            return Parse(new SymbolParserContext(tokens));
        }

        public SymbolParserResponse<T> Parse(SymbolParserContext context)
        {
            SymbolNode<T>? result = new SymbolMatcher<T>().Build(context, _productionRules);

            return new SymbolParserResponse<T>
            {
                Nodes = result,
                DebugStack = context.DebugStack.ToList(),
            };
        }
    }
}
