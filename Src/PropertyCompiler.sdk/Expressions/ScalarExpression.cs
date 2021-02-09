﻿using PropertyCompiler.sdk.Grammar;
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
    /// objectName = {
    ///     Name1=Value1[,
    ///     Name2=Value2, ...]
    ///     };
    ///     
    /// </summary>
    /// 
    public class ScalarAssignmentExpressionBuilder : IExpressionBuilder
    {
        private static readonly CodeBlock<SymbolType> _processingRules = new CodeBlock<SymbolType>()
        {
            new CodeBlock<SymbolType>()
                + Symbols.VariableName
                + Symbols.Equal
                + Symbols.Constant
                + Symbols.SemiColon
        };

        public CodeBlock<SymbolType> ProcessingRules => _processingRules;

        public SyntaxNode? Create(SyntaxTree syntaxTree)
        {
            SymbolParserResponse<SymbolType> response = new SymbolParser<SymbolType>(_processingRules)
                .Parse(syntaxTree.SymbolParserContext);

            if (response.Nodes == null) return null;

            var stack = new Stack<ISymbolToken>(response.Nodes.Reverse<ISymbolToken>());

            string variable = stack.GetNextValue().Value;
            string constant = stack.GetNextValue().Value;

            return new ScalarAssignmentExpression(syntaxTree, variable, constant);
        }
    }

    public class ScalarAssignmentExpression : SyntaxNode
    {
        public ScalarAssignmentExpression(SyntaxTree syntaxTree, string variableName, string? constant)
            : base(syntaxTree)
        {
            variableName.VerifyNotNull(nameof(variableName));

            VariableName = variableName;
            Constant = constant;
        }

        public string VariableName { get; }

        public string? Constant { get; }
    }
}
