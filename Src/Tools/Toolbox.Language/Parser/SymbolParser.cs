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
        private readonly Action<string> _logger;
        private readonly TokenParser<T> _tokenizer;

        public SymbolParser(CodeBlock<T> productionRules, Action<string>? logger = null)
        {
            productionRules.VerifyNotNull(nameof(productionRules));

            _productionRules = productionRules;
            _logger = logger ?? (x => { });
            _tokenizer = new TokenParser<T>(_productionRules);
        }

        public SymbolNode<T>? Parse(params string[] rawLines)
        {
            string rawData = rawLines.Aggregate(string.Empty, (a, x) => a += x);

            IReadOnlyList<IToken> tokens = _tokenizer.Parse(rawData);
            if (tokens.Count == 0) return null;

            return Parse(new SymbolParserContext(tokens, _logger));
        }

        public SymbolNode<T>? Parse(SymbolParserContext context)
        {
            foreach (ICodeBlock<T> rule in _productionRules)
            {
                SymbolNode<T>? syntaxNode = rule.Build(context);
                if (syntaxNode != null) return syntaxNode;
            }

            return null;
        }
    }
}
