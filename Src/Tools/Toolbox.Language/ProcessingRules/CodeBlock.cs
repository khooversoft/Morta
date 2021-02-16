using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Toolbox.Extensions;
using Toolbox.Language.Grammar;
using Toolbox.Language.Parser;
using Toolbox.Tokenizer.Token;
using Toolbox.Tools;

namespace Toolbox.Language.ProcessingRules
{
    public class CodeBlock<T> : List<IGrammar<T>>, ICodeBlock<T> where T : Enum
    {
        public CodeBlock()
        {
        }

        public CodeBlock(string name)
        {
            Name = name;
        }

        public string? Name { get; }

        public IReadOnlyList<IGrammarToken<T>> GetGrammars() => this.Flatten<IGrammar<T>>().OfType<IGrammarToken<T>>().ToList();

        public SymbolNode<T>? Build(SymbolParserContext context)
        {
            context.LogStarting<T>(this);

            SymbolNode<T>? result = new SymbolMatcher<T>().Build(context, this);

            if (result != null)
                context.LogCompleted<T>(this);
            else

                context.LogFail<T>(this);

            return result;
        }


        public override string ToString() => $"{nameof(CodeBlock<T>)}, Name={Name}, Count={Count}";

        public static CodeBlock<T> operator +(CodeBlock<T> left, IGrammar<T> right)
        {
            left.Add(right);
            return left;
        }

        public static CodeBlock<T> operator +(CodeBlock<T> left, IEnumerable<ICodeBlock<T>> right)
        {
            left.VerifyNotNull(nameof(left));
            right.VerifyNotNull(nameof(right));

            right.ForEach(x => left.Add(x));
            return left;
        }
    }
}