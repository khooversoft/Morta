using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Language.Grammar;
using Toolbox.Language.Parser;
using Toolbox.Tools;

namespace Toolbox.Language.ProcessingRules
{
    public class Reference<T> : ICodeBlock<T> where T : Enum
    {
        private CodeBlock<T>? _codeBlock;

        public Reference() 
        {
        }

        public Reference(string name)
        {
            Name = name;
        }

        public string? Name { get; }

        public SymbolNode<T>? Build(SymbolParserContext context)
        {
            _codeBlock.VerifyNotNull("Code block not set");

            context.LogStarting<T>(this);

            SymbolNode<T>? result = new SymbolMatcher<T>().Build(context, _codeBlock);

            if (result != null)
                context.LogCompleted<T>(this);
            else
                context.LogFail<T>(this);

            return result;
        }

        public void Set(CodeBlock<T> codeBlock) => _codeBlock = codeBlock;

        public override string ToString() => $"{nameof(Reference<T>)}, {_codeBlock?.ToString() ?? "<not set>"}";


        public IEnumerator<IGrammar<T>> GetEnumerator() => Enumerable.Empty<ICodeBlock<T>>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
