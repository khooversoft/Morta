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
    public class Optional<T> : List<IGrammar<T>>, ICodeBlock<T> where T : Enum
    {
        public Optional()
        {
        }

        public Optional(string name)
        {
            Name = name;
        }

        public string? Name { get; }

        public SymbolNode<T>? Build(SymbolParserContext context)
        {
            context.LogStarting<T>(this);

            SymbolNode<T>? result = new SymbolMatcher<T>().Build(context, this);

            if (result != null)
                context.LogCompleted<T>(this);
            else
                context.LogSkipped<T>(this);

            // Optional is never a failure
            return result ?? new SymbolNode<T>();
        }

        public override string ToString() => $"{nameof(Optional<T>)}, Name={Name}, Count ={Count}";

        public static Optional<T> operator +(Optional<T> left, IGrammar<T> right)
        {
            left.Add(right);
            return left;
        }
    }
}
