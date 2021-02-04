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

        public SymbolNode<T>? Build(SymbolParserContext context)
        {
            _codeBlock.VerifyNotNull("Code block not set");

            return new SymbolMatcher<T>().Build(context, _codeBlock);
        }

        public void Set(CodeBlock<T> codeBlock) => _codeBlock = codeBlock;

        public IEnumerator<IGrammar<T>> GetEnumerator() => Enumerable.Empty<ICodeBlock<T>>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
