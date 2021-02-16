using PropertyCompiler.sdk.Grammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Language.ProcessingRules;

namespace PropertyCompiler.sdk.Syntax
{
    public interface IExpressionBuilder
    {
        public CodeBlock<SymbolType> ProcessingRules { get; }

        public SyntaxResponse Create(SyntaxTree syntaxTree);
    }
}
