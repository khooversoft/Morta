using PropertyCompiler.sdk.Grammar;
using PropertyCompiler.sdk.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using Toolbox.Language.Parser;
using Toolbox.Language.ProcessingRules;

namespace PropertyCompiler.sdk.Expressions
{
    /// <summary>
    /// 
    /// resource resourceId = filePath;
    ///     
    /// </summary>
    /// 
    public class ResourceExpressionBuilder : IExpressionBuilder
    {
        private static readonly CodeBlock<SymbolType> _processingRules = new CodeBlock<SymbolType>()
        {
            new CodeBlock<SymbolType>()
                + SymbolSyntax.Resource
                + SymbolSyntax.VariableName
                + SymbolSyntax.Equal
                + SymbolSyntax.Constant
                + SymbolSyntax.SemiColon
        };

        public CodeBlock<SymbolType> ProcessingRules => _processingRules;

        public SyntaxResponse Create(SyntaxTree syntaxTree)
        {
            SymbolParserResponse<SymbolType> response = new SymbolParser<SymbolType>(_processingRules)
                .Parse(syntaxTree.SymbolParserContext);

            if (response.Nodes == null) return new SyntaxResponse { DebugStack = response.DebugStack };

            var stack = new Stack<ISymbolToken>(response.Nodes.Reverse<ISymbolToken>());

            SymbolValue<SymbolType> resourceId = stack.GetNextValue();
            SymbolValue<SymbolType> filePath = stack.GetNextValue();

            return new SyntaxResponse
            {
                SyntaxNode = new ResourceExpression(resourceId, filePath),
                DebugStack = response.DebugStack
            };
        }
    }
}
