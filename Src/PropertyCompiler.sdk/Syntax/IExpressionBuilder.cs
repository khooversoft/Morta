using PropertyCompiler.sdk.Grammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Parser;

namespace PropertyCompiler.sdk.Syntax
{
    public interface IExpressionBuilder
    {
        public RuleBlock<SymbolType> ProcessingRules { get; }

        public SyntaxNode? Create(SyntaxTree syntaxTree);
    }
}
