using PropertyCompiler.sdk.Grammar;
using PropertyCompiler.sdk.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Extensions;
using Toolbox.Language.Parser;
using Toolbox.Language.ProcessingRules;
using Toolbox.Tools;

namespace PropertyCompiler.sdk.Expressions
{
    /// <summary>
    /// 
    /// assembly file;
    ///     
    /// </summary>
    /// 
    public class AssemblyExpressionBuilder : IExpressionBuilder
    {
        private static readonly RuleBlock<SymbolType> _processingRules = new RuleBlock<SymbolType>()
        {
            new CodeBlock<SymbolType>()
                + Symbols.Assembly
                + Symbols.Constant
                + Symbols.SemiColon
        };

        public RuleBlock<SymbolType> ProcessingRules => _processingRules;

        public SyntaxNode? Create(SyntaxTree syntaxTree)
        {
            SymbolNode<SymbolType>? symbolParser = new SymbolParser<SymbolType>(_processingRules)
                .Parse(syntaxTree.SymbolParserContext);

            if (symbolParser == null) return null;

            var stack = new Stack<ISymbolToken>(symbolParser.Reverse<ISymbolToken>());

            string assemblyPath = stack.GetNextValue().Value;

            return new AssemblyExpression(syntaxTree, assemblyPath);
        }
    }

    public class AssemblyExpression : SyntaxNode
    {
        public AssemblyExpression(SyntaxTree syntaxTree, string assemblyPath)
            : base(syntaxTree)
        {
            assemblyPath.VerifyNotNull(nameof(assemblyPath));

            AssemblyPath = assemblyPath;
        }

        public string AssemblyPath { get; }
    }
}
