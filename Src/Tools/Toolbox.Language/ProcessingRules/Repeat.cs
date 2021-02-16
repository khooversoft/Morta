using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Extensions;
using Toolbox.Language.Grammar;
using Toolbox.Language.Parser;
using Toolbox.Tokenizer.Token;

namespace Toolbox.Language.ProcessingRules
{
    public class Repeat<T> : List<IGrammar<T>>, ICodeBlock<T> where T : Enum
    {
        public Repeat()
        {
        }

        public Repeat(string name)
        {
            Name = name;
        }

        public string? Name { get; }

        public SymbolNode<T>? Build(SymbolParserContext context)
        {
            var matcher = new SymbolMatcher<T>();
            var syntaxNode = new SymbolNode<T>();

            context.LogStarting<T>(this);

            while (true)
            {
                SymbolNode<T>? result = matcher.Build(context, this);
                if (result == null) break;

                syntaxNode += result;
            }

            context.LogCompleted<T>(this);
            return syntaxNode;
        }

        public override string ToString() => $"{nameof(Repeat<T>)}, Name={Name}, Count={Count}";

        public static Repeat<T> operator +(Repeat<T> left, IGrammar<T> right)
        {
            left.Add(right);
            return left;
        }
    }
}
