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
    /// include file;
    ///     
    /// </summary>
    /// 
    public class IncludeExpressionBuilder : IExpressionBuilder
    {
        private static readonly CodeBlock<SymbolType> _processingRules = new CodeBlock<SymbolType>()
        {
            new CodeBlock<SymbolType>()
                + Symbols.Include
                + Symbols.Constant
                + Symbols.SemiColon
        };

        public CodeBlock<SymbolType> ProcessingRules => _processingRules;

        public SyntaxNode? Create(SyntaxTree syntaxTree)
        {
            SymbolParserResponse<SymbolType> symbolParser = new SymbolParser<SymbolType>(_processingRules)
                .Parse(syntaxTree.SymbolParserContext);

            if (symbolParser.Nodes == null) return null;

            var stack = new Stack<ISymbolToken>(symbolParser.Nodes.Reverse<ISymbolToken>());

            string includePath = stack.GetNextValue().Value;

            return new IncludeExpression(syntaxTree, includePath);
        }
    }

    public class IncludeExpression : SyntaxNode
    {
        public IncludeExpression(SyntaxTree syntaxTree, string includePath)
            : base(syntaxTree)
        {
            includePath.VerifyNotNull(nameof(includePath));

            IncludePath = includePath;
        }

        public string IncludePath { get; }
    }
}
