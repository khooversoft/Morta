using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Language.Grammar;
using Toolbox.Language.Parser;

namespace Toolbox.Language.ProcessingRules
{
    public class Optional<T> : List<IGrammar<T>>, IRuleBlock<T> where T : Enum
    {
        public SymbolNode<T>? Build(SymbolParserContext context) => new SymbolMatcher<T>().Build(context, this) ?? new SymbolNode<T>();

        public override string ToString() => $"{nameof(Optional<T>)}, Count={Count}";

        public static Optional<T> operator +(Optional<T> left, IGrammar<T> right)
        {
            left.Add(right);
            return left;
        }
    }
}
