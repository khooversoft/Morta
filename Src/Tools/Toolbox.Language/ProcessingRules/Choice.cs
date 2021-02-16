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
    public class Choice<T> : List<IGrammar<T>>, ICodeBlock<T> where T : Enum
    {
        public Choice()
        {
        }

        public Choice(string name)
        {
            Name = name;
        }

        public string? Name { get; }

        public SymbolNode<T>? Build(SymbolParserContext context)
        {
            var matcher = new SymbolMatcher<T>();

            context.LogStarting<T>(this);

            foreach (var item in this)
            {
                var ruleNode = new CodeBlock<T>(Name + ".tmp") + item;
                SymbolNode<T>? result = matcher.Build(context, ruleNode);

                if (result != null)
                {
                    context.LogCompleted<T>(this);
                    return result;
                }
            }

            context.LogFail<T>(this);
            return default;
        }

        public override string ToString() => $"{nameof(Choice<T>)}, Name={Name}, Count={Count}";

        public static Choice<T> operator +(Choice<T> left, IGrammar<T> right)
        {
            left.Add(right);
            return left;
        }
    }
}
