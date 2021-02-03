using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolbox.Extensions;
using Toolbox.Language.Grammar;
using Toolbox.Language.Parser;
using Toolbox.Language.ProcessingRules;
using Toolbox.Tokenizer.Token;
using Toolbox.Tools;

namespace Toolbox.Parser
{
    public class CodeBlock<T> : List<IGrammar<T>>, IRuleBlock<T> where T : Enum
    {
        public CodeBlock()
        {
        }

        public SymbolNode<T>? Build(SymbolParserContext context) => new SymbolMatcher<T>().Build(context, this);

        public static CodeBlock<T> operator +(CodeBlock<T> left, IGrammar<T> right)
        {
            left.Add(right);
            return left;
        }
    }
}