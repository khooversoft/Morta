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

namespace PropertyCompiler.sdk.Expressions
{
    /// <summary>
    /// 
    /// objectName = {
    ///     Name1=Value1[,
    ///     Name2=Value2, ...]
    ///     };
    ///     
    /// </summary>
    /// 
    public class ScalarAssignmentBuilder : IExpressionBuilder
    {
        private static readonly CodeBlock<SymbolType> _processingRules = new CodeBlock<SymbolType>()
        {
            new CodeBlock<SymbolType>()
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

            SymbolValue<SymbolType> variable = stack.GetNextValue();
            SymbolValue<SymbolType> constant = stack.GetNextValue();

            return new SyntaxResponse
            {
                SyntaxNode = new ScalarAssignment(variable, constant),
                DebugStack = response.DebugStack
            };
        }
    }
}
