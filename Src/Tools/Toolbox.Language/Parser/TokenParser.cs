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
    public class TokenParser<T> where T : Enum
    {
        private readonly StringTokenizer _tokenizer;

        public TokenParser(RuleBlock<T> productionRules)
        {
            productionRules.VerifyNotNull(nameof(productionRules));

            _tokenizer = new StringTokenizer()
                .UseCollapseWhitespace()
                .UseDoubleQuote()
                .UseSingleQuote();

            productionRules.GetGrammars()
                .Where(x => x.Flags != GrammarFlags.Keyword)
                .Select(x => x.Match)
                .Distinct()
                .ForEach(x => _tokenizer.Add(x));
        }

        public IReadOnlyList<IToken> Parse(params string[] rawLines)
        {
            string rawData = rawLines.Aggregate(string.Empty, (a, x) => a += x);

            return _tokenizer.Parse(rawData)
                .Where(x => !(x.TokenType == TokenType.ParseToken && x.Value == " "))
                .ToList();
        }
    }
}
